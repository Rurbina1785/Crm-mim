// CRM System JavaScript with HTMX Integration

// Global variables
let currentPage = 1;
let pageSize = 10;
let currentFilters = {};

// API Base URL
const API_BASE = '/api';

// Initialize application
document.addEventListener('DOMContentLoaded', function() {
    initializeApp();
});

// Initialize the application
async function initializeApp() {
    try {
        await loadDashboard();
        await loadLookupData();
        setupEventListeners();
        showNotification('Sistema CRM cargado correctamente', 'success');
    } catch (error) {
        console.error('Error initializing app:', error);
        showNotification('Error al cargar el sistema', 'error');
    }
}

// Load dashboard data
async function loadDashboard() {
    try {
        // Load statistics
        const [leadsStats, clientsStats] = await Promise.all([
            fetch(`${API_BASE}/leads/statistics`).then(r => r.json()),
            fetch(`${API_BASE}/clients/statistics`).then(r => r.json())
        ]);

        // Update stats cards
        document.getElementById('total-leads').textContent = leadsStats.TotalLeads || 0;
        document.getElementById('total-clients').textContent = clientsStats.TotalClients || 0;
        document.getElementById('leads-count').textContent = leadsStats.TotalLeads || 0;
        document.getElementById('clients-count').textContent = clientsStats.TotalClients || 0;

        // Calculate conversion rate
        const conversionRate = leadsStats.TotalLeads > 0 
            ? Math.round((leadsStats.ConvertedLeads / leadsStats.TotalLeads) * 100)
            : 0;
        document.getElementById('conversion-rate').textContent = `${conversionRate}%`;

        // Load charts
        loadLeadsStatusChart(leadsStats.LeadsByStatus || []);
        loadLeadsSourceChart(leadsStats.LeadsBySource || []);

    } catch (error) {
        console.error('Error loading dashboard:', error);
        showNotification('Error al cargar el dashboard', 'error');
    }
}

// Load lookup data for dropdowns
async function loadLookupData() {
    try {
        // This would typically load from lookup endpoints
        // For now, we'll use static data
        const sources = [
            { id: 1, name: 'Sitio Web' },
            { id: 2, name: 'Referencia' },
            { id: 3, name: 'Redes Sociales' },
            { id: 4, name: 'Email Marketing' },
            { id: 5, name: 'Llamada Fría' },
            { id: 6, name: 'Exposición/Feria' },
            { id: 7, name: 'Campaña Marketing' }
        ];

        const statuses = [
            { id: 1, name: 'Nuevo' },
            { id: 2, name: 'Contactado' },
            { id: 3, name: 'Calificado' },
            { id: 4, name: 'Propuesta' },
            { id: 5, name: 'Negociación' },
            { id: 6, name: 'Cerrado Ganado' },
            { id: 7, name: 'Cerrado Perdido' }
        ];

        // Populate dropdowns
        populateSelect('filter-source', sources);
        populateSelect('filter-status', statuses);
        populateSelect('[name="sourceId"]', sources);
        populateSelect('[name="statusId"]', statuses);

    } catch (error) {
        console.error('Error loading lookup data:', error);
    }
}

// Populate select elements
function populateSelect(selector, options) {
    const selects = document.querySelectorAll(selector);
    selects.forEach(select => {
        // Keep existing options (like "Todos" or "Seleccionar")
        const existingOptions = Array.from(select.options);
        
        options.forEach(option => {
            const optionElement = document.createElement('option');
            optionElement.value = option.id;
            optionElement.textContent = option.name;
            select.appendChild(optionElement);
        });
    });
}

// Setup event listeners
function setupEventListeners() {
    // HTMX event listeners
    document.body.addEventListener('htmx:beforeRequest', function(evt) {
        showLoading(true);
    });

    document.body.addEventListener('htmx:afterRequest', function(evt) {
        showLoading(false);
        if (evt.detail.xhr.status >= 400) {
            showNotification('Error en la solicitud', 'error');
        }
    });

    // Form submissions
    document.addEventListener('submit', function(e) {
        if (e.target.matches('#leadForm')) {
            e.preventDefault();
            saveLead();
        }
    });
}

// Show/hide sections
function showSection(sectionName) {
    // Hide all sections
    document.querySelectorAll('.content-section').forEach(section => {
        section.classList.add('d-none');
    });

    // Show selected section
    const targetSection = document.getElementById(`${sectionName}-section`);
    if (targetSection) {
        targetSection.classList.remove('d-none');
        targetSection.classList.add('fade-in');
    }

    // Update navigation
    document.querySelectorAll('#main-nav .nav-link').forEach(link => {
        link.classList.remove('active');
    });
    
    const activeLink = document.querySelector(`#main-nav .nav-link[onclick*="${sectionName}"]`);
    if (activeLink) {
        activeLink.classList.add('active');
    }

    // Load section-specific data
    switch (sectionName) {
        case 'leads':
            loadLeads();
            break;
        case 'clients':
            loadClients();
            break;
        case 'dashboard':
            loadDashboard();
            break;
    }
}

// Load leads data
async function loadLeads(page = 1) {
    try {
        const params = new URLSearchParams({
            page: page,
            pageSize: pageSize,
            ...currentFilters
        });

        const response = await fetch(`${API_BASE}/leads?${params}`);
        const leads = await response.json();
        
        // Get pagination info from headers
        const totalCount = parseInt(response.headers.get('X-Total-Count') || '0');
        const currentPageNum = parseInt(response.headers.get('X-Page') || '1');
        
        renderLeadsTable(leads);
        renderPagination('leads-pagination', currentPageNum, Math.ceil(totalCount / pageSize), loadLeads);
        
    } catch (error) {
        console.error('Error loading leads:', error);
        showNotification('Error al cargar los leads', 'error');
    }
}

// Render leads table
function renderLeadsTable(leads) {
    const tbody = document.getElementById('leads-table-body');
    
    if (leads.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-muted py-4">
                    <i class="fas fa-inbox fa-2x mb-2"></i><br>
                    No se encontraron leads
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = leads.map(lead => `
        <tr>
            <td><strong>${lead.leadCode || 'N/A'}</strong></td>
            <td>${lead.firstName} ${lead.lastName}</td>
            <td>${lead.companyName || '-'}</td>
            <td>${lead.email || '-'}</td>
            <td>
                <span class="badge bg-secondary">${lead.source?.sourceName || 'N/A'}</span>
            </td>
            <td>
                <span class="badge ${getStatusBadgeClass(lead.status?.statusName)}">${lead.status?.statusName || 'N/A'}</span>
            </td>
            <td>
                <div class="progress" style="height: 20px;">
                    <div class="progress-bar" role="progressbar" style="width: ${lead.score || 0}%">
                        ${lead.score || 0}%
                    </div>
                </div>
            </td>
            <td>
                <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-primary" onclick="viewLead(${lead.id})" title="Ver">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-outline-success" onclick="editLead(${lead.id})" title="Editar">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-outline-warning" onclick="convertLead(${lead.id})" title="Convertir">
                        <i class="fas fa-user-check"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Get status badge class
function getStatusBadgeClass(status) {
    const statusClasses = {
        'Nuevo': 'bg-info',
        'Contactado': 'bg-primary',
        'Calificado': 'bg-warning',
        'Propuesta': 'bg-secondary',
        'Negociación': 'bg-warning',
        'Cerrado Ganado': 'bg-success',
        'Cerrado Perdido': 'bg-danger'
    };
    return statusClasses[status] || 'bg-secondary';
}

// Render pagination
function renderPagination(containerId, currentPage, totalPages, loadFunction) {
    const container = document.getElementById(containerId);
    
    if (totalPages <= 1) {
        container.innerHTML = '';
        return;
    }

    let pagination = '';
    
    // Previous button
    pagination += `
        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="${loadFunction.name}(${currentPage - 1}); return false;">
                <i class="fas fa-chevron-left"></i>
            </a>
        </li>
    `;

    // Page numbers
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    if (startPage > 1) {
        pagination += `<li class="page-item"><a class="page-link" href="#" onclick="${loadFunction.name}(1); return false;">1</a></li>`;
        if (startPage > 2) {
            pagination += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        pagination += `
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#" onclick="${loadFunction.name}(${i}); return false;">${i}</a>
            </li>
        `;
    }

    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            pagination += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
        pagination += `<li class="page-item"><a class="page-link" href="#" onclick="${loadFunction.name}(${totalPages}); return false;">${totalPages}</a></li>`;
    }

    // Next button
    pagination += `
        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="${loadFunction.name}(${currentPage + 1}); return false;">
                <i class="fas fa-chevron-right"></i>
            </a>
        </li>
    `;

    container.innerHTML = pagination;
}

// Filter leads
function filterLeads() {
    currentFilters = {
        statusId: document.getElementById('filter-status').value,
        sourceId: document.getElementById('filter-source').value,
        search: document.getElementById('search-leads').value
    };

    // Remove empty filters
    Object.keys(currentFilters).forEach(key => {
        if (!currentFilters[key]) {
            delete currentFilters[key];
        }
    });

    loadLeads(1);
}

// Clear filters
function clearFilters() {
    document.getElementById('filter-status').value = '';
    document.getElementById('filter-source').value = '';
    document.getElementById('search-leads').value = '';
    currentFilters = {};
    loadLeads(1);
}

// Save lead
async function saveLead() {
    const form = document.getElementById('leadForm');
    const formData = new FormData(form);
    
    const leadData = {
        firstName: formData.get('firstName'),
        lastName: formData.get('lastName'),
        companyName: formData.get('companyName'),
        email: formData.get('email'),
        phone: formData.get('phone'),
        sourceId: parseInt(formData.get('sourceId')),
        statusId: parseInt(formData.get('statusId')) || 1, // Default to "Nuevo"
        estimatedValue: parseFloat(formData.get('estimatedValue')) || 0,
        notes: formData.get('notes'),
        branchId: 1, // Default branch
        assignedUserId: 1 // Default user
    };

    try {
        showLoading(true);
        
        const response = await fetch(`${API_BASE}/leads`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(leadData)
        });

        if (response.ok) {
            const newLead = await response.json();
            showNotification('Lead creado exitosamente', 'success');
            
            // Close modal and refresh data
            const modal = bootstrap.Modal.getInstance(document.getElementById('leadModal'));
            modal.hide();
            form.reset();
            
            // Refresh leads if we're on the leads section
            if (!document.getElementById('leads-section').classList.contains('d-none')) {
                loadLeads();
            }
            
            // Refresh dashboard
            loadDashboard();
            
        } else {
            const error = await response.text();
            showNotification(`Error al crear el lead: ${error}`, 'error');
        }
        
    } catch (error) {
        console.error('Error saving lead:', error);
        showNotification('Error al guardar el lead', 'error');
    } finally {
        showLoading(false);
    }
}

// Load clients (placeholder)
async function loadClients() {
    const content = document.getElementById('clients-content');
    content.innerHTML = `
        <div class="card">
            <div class="card-body text-center">
                <i class="fas fa-users fa-3x text-muted mb-3"></i>
                <h5>Gestión de Clientes</h5>
                <p class="text-muted">Esta sección estará disponible próximamente.</p>
            </div>
        </div>
    `;
}

// Chart functions
function loadLeadsStatusChart(data) {
    const ctx = document.getElementById('leadsStatusChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: data.map(item => item.Status),
            datasets: [{
                data: data.map(item => item.Count),
                backgroundColor: [
                    '#0dcaf0', '#0d6efd', '#ffc107', '#6c757d', 
                    '#fd7e14', '#198754', '#dc3545'
                ],
                borderWidth: 2,
                borderColor: '#fff'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

function loadLeadsSourceChart(data) {
    const ctx = document.getElementById('leadsSourceChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.map(item => item.Source),
            datasets: [{
                label: 'Cantidad de Leads',
                data: data.map(item => item.Count),
                backgroundColor: 'rgba(13, 110, 253, 0.8)',
                borderColor: 'rgba(13, 110, 253, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
}

// Utility functions
function showLoading(show) {
    const loadingElements = document.querySelectorAll('.loading');
    loadingElements.forEach(el => {
        el.style.display = show ? 'inline-block' : 'none';
    });
}

function showNotification(message, type = 'info') {
    const container = document.getElementById('notification-container');
    const alertClass = {
        'success': 'alert-success',
        'error': 'alert-danger',
        'warning': 'alert-warning',
        'info': 'alert-info'
    }[type] || 'alert-info';

    const notification = document.createElement('div');
    notification.className = `alert ${alertClass} alert-dismissible fade show notification`;
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    container.appendChild(notification);

    // Auto-remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

function refreshDashboard() {
    loadDashboard();
    showNotification('Dashboard actualizado', 'success');
}

// Lead action functions (placeholders)
function viewLead(id) {
    showNotification(`Ver lead ${id} - Funcionalidad en desarrollo`, 'info');
}

function editLead(id) {
    showNotification(`Editar lead ${id} - Funcionalidad en desarrollo`, 'info');
}

function convertLead(id) {
    showNotification(`Convertir lead ${id} - Funcionalidad en desarrollo`, 'info');
}
