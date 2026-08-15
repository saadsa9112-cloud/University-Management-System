/* NED Academy – site.js | University Management System */

'use strict';

// ════════════════════════════════════════════
// SIDEBAR TOGGLE
// ════════════════════════════════════════════
const sidebar       = document.getElementById('sidebar');
const mainWrapper   = document.getElementById('mainWrapper');
const sidebarOverlay= document.getElementById('sidebarOverlay');
const navToggle     = document.getElementById('navToggle');

let isMobile     = () => window.innerWidth <= 1100;
let collapsed    = localStorage.getItem('sidebarCollapsed') === 'true';

function applySidebar() {
  if (isMobile()) {
    sidebar?.classList.toggle('mobile-open', !collapsed);
    sidebarOverlay?.classList.toggle('show', !collapsed);
    sidebar?.classList.remove('collapsed');
    mainWrapper?.classList.remove('collapsed');
  } else {
    sidebar?.classList.remove('mobile-open');
    sidebarOverlay?.classList.remove('show');
    sidebar?.classList.toggle('collapsed', collapsed);
    mainWrapper?.classList.toggle('collapsed', collapsed);
  }
}

navToggle?.addEventListener('click', () => {
  collapsed = !collapsed;
  localStorage.setItem('sidebarCollapsed', collapsed);
  applySidebar();
});

sidebarOverlay?.addEventListener('click', () => {
  collapsed = true;
  applySidebar();
});

if (sidebar) {
  applySidebar();
  window.addEventListener('resize', applySidebar);
}

// ════════════════════════════════════════════
// ACTIVE NAV DETECTION
// ════════════════════════════════════════════
(function setActiveNav() {
  const path = window.location.pathname.toLowerCase();
  document.querySelectorAll('.nav-link-item').forEach(link => {
    const href = (link.getAttribute('href') || '').toLowerCase();
    if (!href) return;
    const isHome = href === '/' || href.includes('/home');
    const isActive = isHome
      ? (path === '/' || path.startsWith('/home'))
      : (href !== '/' && path.startsWith(href));
    if (isActive) link.classList.add('active');
    else link.classList.remove('active');
  });
})();

// ════════════════════════════════════════════
// LOADING OVERLAY
// ════════════════════════════════════════════
const overlay = document.getElementById('loadingOverlay');

window.showLoading = () => overlay?.classList.add('show');
window.hideLoading = () => overlay?.classList.remove('show');

document.addEventListener('submit', e => {
  if (!e.target.hasAttribute('data-no-loading')) showLoading();
});

// ════════════════════════════════════════════
// SWEETALERT DELETE CONFIRMATION
// ════════════════════════════════════════════
window.confirmDelete = function(form, name = 'this record') {
  Swal.fire({
    title: 'Delete ' + name + '?',
    text: 'This action cannot be undone.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#ef4444',
    cancelButtonColor:  '#6b7280',
    confirmButtonText: '<i class="fas fa-trash me-1"></i> Yes, Delete!',
    cancelButtonText:  'Cancel',
    reverseButtons: true,
    focusCancel: true,
    customClass: { popup: 'rounded-3' },
  }).then(r => {
    if (r.isConfirmed) { showLoading(); form.submit(); }
  });
};

// ════════════════════════════════════════════
// TOASTR CONFIGURATION
// ════════════════════════════════════════════
if (typeof toastr !== 'undefined') {
  toastr.options = {
    closeButton:   true,
    progressBar:   true,
    positionClass: 'toast-top-right',
    timeOut:       4000,
    extendedTimeOut: 800,
    showEasing:    'swing',
    hideEasing:    'linear',
    showMethod:    'fadeIn',
    hideMethod:    'fadeOut',
    preventDuplicates: true,
  };
}

// ════════════════════════════════════════════
// IMAGE PREVIEW
// ════════════════════════════════════════════
window.previewImage = function(input, previewId, placeholderId) {
  if (!input.files?.length) return;
  const reader = new FileReader();
  reader.onload = e => {
    const preview = document.getElementById(previewId);
    if (preview) { preview.src = e.target.result; preview.style.display = 'block'; }
    const ph = document.getElementById(placeholderId);
    if (ph) ph.style.display = 'none';
  };
  reader.readAsDataURL(input.files[0]);
};

// ════════════════════════════════════════════
// COUNTER ANIMATION (dashboard stat cards)
// ════════════════════════════════════════════
window.animateCounter = function(el, target, duration = 1400) {
  if (!el) return;
  const start = performance.now();
  const easeOut = t => 1 - Math.pow(1 - t, 3);
  (function update(now) {
    const p = Math.min((now - start) / duration, 1);
    el.textContent = Math.round(easeOut(p) * target).toLocaleString();
    if (p < 1) requestAnimationFrame(update);
  })(start);
};

// ════════════════════════════════════════════
// DATATABLES INIT HELPER
// ════════════════════════════════════════════
window.initDataTable = function(tableId, opts = {}) {
  if (typeof $ === 'undefined' || !$.fn.DataTable) return;
  const defaults = {
    pageLength:  10,
    responsive:  true,
    lengthMenu:  [[10, 25, 50, -1], [10, 25, 50, 'All']],
    dom: "<'row mb-2'<'col-sm-6'l><'col-sm-6 d-flex justify-content-end'f>>" +
         "<'row'<'col-sm-12'tr>>" +
         "<'row mt-2'<'col-sm-5'i><'col-sm-7'p>>",
    language: {
      search: '',
      searchPlaceholder: 'Search records...',
      emptyTable: 'No records found',
      zeroRecords: 'No matching records',
      info: 'Showing _START_–_END_ of _TOTAL_ entries',
      infoEmpty: 'No entries to show',
    },
  };
  $(`#${tableId}`).DataTable({ ...defaults, ...opts });
};

// ════════════════════════════════════════════
// AUTO-DISMISS ALERTS
// ════════════════════════════════════════════
document.querySelectorAll('.alert.auto-dismiss').forEach(el => {
  setTimeout(() => {
    el.style.opacity = '0';
    setTimeout(() => el.remove(), 500);
  }, 5000);
});
