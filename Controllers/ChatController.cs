using EducareSA.Data;
using EducareSA.Services;
using EducareSA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducareSA.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILlmService _llm;
        private readonly EducareDbContext _context;

        public ChatController(ILlmService llm, EducareDbContext context)
        {
            _llm = llm;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatResponse>> Send([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return Ok(new ChatResponse { Success = false, Error = "Please type a message." });

            // Build a compact context of what's in the DB, so the assistant
            // can answer questions about the actual catalogue.
            var context = await BuildContextAsync();

            var systemPrompt = $@"You are the EducareSA assistant — a friendly, concise helper for South African matric learners exploring university programmes and bursaries.

Rules:
- Keep replies short (2-4 sentences unless asked for detail).
- Only state facts that appear in the context below. If you don't know, say so.
- When recommending programmes or bursaries, name them exactly as listed in the context.
- Never invent requirements, fees, or deadlines.
- Be encouraging but honest.
- If the user asks something off-topic (not about studying, universities, programmes, bursaries, APS, or applying in South Africa), politely redirect.

Context about what EducareSA has in its database right now:
{context}";

            var history = new List<(string Role, string Content)>
            {
                ("user", request.Message)
            };

            var reply = await _llm.AskAsync(systemPrompt, history);

            return Ok(new ChatResponse { Reply = reply, Success = true });
        }

        private async Task<string> BuildContextAsync()
        {
            var universities = await _context.Universities
                .Where(u => u.IsActive)
                .OrderBy(u => u.Name)
                .Select(u => u.Name + " (" + u.ShortName + ") — " + u.City + ", " + u.Province)
                .Take(30)
                .ToListAsync();

            var programmes = await _context.Programmes
                .Where(p => p.IsActive)
                .Include(p => p.Faculty).ThenInclude(f => f.University)
                .OrderBy(p => p.Name)
                .Take(60)
                .Select(p => p.Name + " — " + p.Faculty.University.ShortName + " (" + p.QualificationType + ")")
                .ToListAsync();

            var bursaries = await _context.Bursaries
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .Take(30)
                .Select(b => b.Name + " — " + (b.Provider ?? "unknown provider"))
                .ToListAsync();

            var sb = new System.Text.StringBuilder();

            sb.AppendLine($"Universities ({universities.Count}):");
            foreach (var u in universities) sb.AppendLine("  - " + u);

            sb.AppendLine();
            sb.AppendLine($"Programmes (first {programmes.Count}):");
            foreach (var p in programmes) sb.AppendLine("  - " + p);

            sb.AppendLine();
            sb.AppendLine($"Bursaries ({bursaries.Count}):");
            foreach (var b in bursaries) sb.AppendLine("  - " + b);

            return sb.ToString();
        }
    }
}
