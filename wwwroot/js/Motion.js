/* EducareSA — motion & interaction primitives
   Vanilla, no dependencies. Respects prefers-reduced-motion. */

(function () {
    'use strict';

    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    /* ---------- 1. Theme toggle ---------- */
    function initializeTheme() {
        const root = document.documentElement;
        const stored = localStorage.getItem('educare-theme');
        const systemDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        const theme = stored || (systemDark ? 'dark' : 'light');

        root.setAttribute('data-theme', theme);
        syncToggleIcons(theme);

        const toggles = document.querySelectorAll('[data-theme-toggle]');
        toggles.forEach(btn => {
            btn.addEventListener('click', () => {
                const current = root.getAttribute('data-theme');
                const next = current === 'dark' ? 'light' : 'dark';
                root.setAttribute('data-theme', next);
                localStorage.setItem('educare-theme', next);
                syncToggleIcons(next);
            });
        });
    }

    function syncToggleIcons(theme) {
        document.querySelectorAll('[data-theme-toggle]').forEach(btn => {
            const sun = btn.querySelector('.theme-icon-sun');
            const moon = btn.querySelector('.theme-icon-moon');
            if (!sun || !moon) return;
            sun.style.display = theme === 'dark' ? 'none' : 'inline';
            moon.style.display = theme === 'dark' ? 'inline' : 'none';
        });
    }

    /* ---------- 2. Scroll reveals ---------- */
    function initializeScrollAnimations() {
        if (prefersReducedMotion) {
            document.querySelectorAll('.reveal, .reveal-stagger')
                .forEach(el => el.classList.add('is-visible'));
            return;
        }

        const targets = document.querySelectorAll('.reveal, .reveal-stagger');
        if (!targets.length) return;

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('is-visible');
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.12,
            rootMargin: '0px 0px -60px 0px'
        });

        targets.forEach(el => observer.observe(el));
    }

    /* ---------- 3. Button ripple / press feel ---------- */
    function initializeButtonPress() {
        document.addEventListener('pointerdown', (e) => {
            const btn = e.target.closest('.btn-pill, .btn, .icon-btn');
            if (!btn) return;
            btn.style.transform = 'scale(0.96)';
        });

        document.addEventListener('pointerup', () => {
            document.querySelectorAll('.btn-pill, .btn, .icon-btn')
                .forEach(b => { b.style.transform = ''; });
        });

        document.addEventListener('pointercancel', () => {
            document.querySelectorAll('.btn-pill, .btn, .icon-btn')
                .forEach(b => { b.style.transform = ''; });
        });
    }

    /* ---------- 4. Active nav link highlight ---------- */
    function initializeActiveNav() {
        const path = window.location.pathname.toLowerCase();
        document.querySelectorAll('.site-nav .nav-links a').forEach(link => {
            const href = link.getAttribute('href');
            if (!href) return;
            const clean = href.split('?')[0].toLowerCase();
            if (clean !== '/' && path.startsWith(clean)) {
                link.classList.add('active');
            }
        });
    }

    /* ---------- 5. Mobile nav toggle ---------- */
    function initializeSidebar() {
        const nav = document.getElementById('siteNav');
        const toggle = nav?.querySelector('.nav-toggle');
        if (!nav || !toggle) return;
        toggle.addEventListener('click', () => nav.classList.toggle('open'));
    }

    /* ---------- Boot ---------- */
    function initializeApp() {
        initializeTheme();
        initializeScrollAnimations();
        initializeButtonPress();
        initializeActiveNav();
        initializeSidebar();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializeApp);
    } else {
        initializeApp();
    }
})();