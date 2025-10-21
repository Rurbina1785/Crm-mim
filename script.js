// Sample data
const clientsData = [
    {
        id: 1,
        name: "Constructora ABC",
        contact: "María González",
        status: "cotizado",
        lastContact: "2024-01-15",
        value: 45000,
        phone: "+52 55 1234-5678",
        email: "maria@constructoraabc.com"
    },
    {
        id: 2,
        name: "Desarrollos XYZ",
        contact: "Carlos Rodríguez",
        status: "contactado",
        lastContact: "2024-01-12",
        value: 78000,
        phone: "+52 55 2345-6789",
        email: "carlos@desarrollosxyz.com"
    },
    {
        id: 3,
        name: "Inmobiliaria DEF",
        contact: "Ana Martínez",
        status: "prospecto",
        lastContact: "2024-01-10",
        value: 32000,
        phone: "+52 55 3456-7890",
        email: "ana@inmobiliariadef.com"
    },
    {
        id: 4,
        name: "Grupo Constructor GHI",
        contact: "Luis Hernández",
        status: "cerrado",
        lastContact: "2024-01-08",
        value: 95000,
        phone: "+52 55 4567-8901",
        email: "luis@grupoconstructorghi.com"
    },
    {
        id: 5,
        name: "Proyectos JKL",
        contact: "Patricia López",
        status: "cotizado",
        lastContact: "2024-01-14",
        value: 56000,
        phone: "+52 55 5678-9012",
        email: "patricia@proyectosjkl.com"
    },
    {
        id: 6,
        name: "Edificaciones MNO",
        contact: "Roberto Silva",
        status: "contactado",
        lastContact: "2024-01-11",
        value: 67000,
        phone: "+52 55 6789-0123",
        email: "roberto@edificacionesmno.com"
    }
];

const activitiesData = [
    {
        id: 1,
        type: "call",
        client: "Constructora ABC",
        description: "Llamada de seguimiento para cotización de materiales de construcción",
        timestamp: "2024-01-15 14:30",
        user: "Juan Pérez"
    },
    {
        id: 2,
        type: "meeting",
        client: "Desarrollos XYZ",
        description: "Reunión en sitio para evaluar necesidades del proyecto",
        timestamp: "2024-01-15 10:00",
        user: "Juan Pérez"
    },
    {
        id: 3,
        type: "quote",
        client: "Proyectos JKL",
        description: "Envío de cotización actualizada con descuentos por volumen",
        timestamp: "2024-01-14 16:45",
        user: "Juan Pérez"
    },
    {
        id: 4,
        type: "follow-up",
        client: "Inmobiliaria DEF",
        description: "Programado seguimiento para próxima semana",
        timestamp: "2024-01-14 09:15",
        user: "Sistema"
    }
];

// Enhanced leads data with new fields
const leadsData = [
    {
        id: 1,
        name: "Pedro Ramírez",
        company: "Constructora Norte",
        email: "pedro@constructoranorte.com",
        phone: "+52 55 1111-2222",
        source: "website",
        vendor: "Juan Pérez",
        type: "prospect",
        score: 85,
        status: "qualified"
    },
    {
        id: 2,
        name: "Laura Jiménez",
        company: "Desarrollos Sur",
        email: "laura@desarrollossur.com",
        phone: "+52 55 3333-4444",
        source: "expo",
        vendor: "María García",
        type: "prospect",
        score: 72,
        status: "contacted"
    },
    {
        id: 3,
        name: "Miguel Torres",
        company: "Inmobiliaria Este",
        email: "miguel@inmobiliariaeste.com",
        phone: "+52 55 5555-6666",
        source: "social",
        vendor: "Carlos Ruiz",
        type: "prospect",
        score: 45,
        status: "new"
    },
    {
        id: 4,
        name: "Carmen Vega",
        company: "Proyectos Oeste",
        email: "carmen@proyectosoeste.com",
        phone: "+52 55 7777-8888",
        source: "campaign",
        vendor: "Ana López",
        type: "client",
        score: 90,
        status: "converted"
    },
    {
        id: 5,
        name: "Ricardo Morales",
        company: "Grupo Central",
        email: "ricardo@grupocentral.com",
        phone: "+52 55 9999-0000",
        source: "cold-call",
        vendor: "Juan Pérez",
        type: "prospect",
        score: 38,
        status: "new"
    }
];

const tasksData = [
    {
        id: 1,
        title: "Llamar a Constructora ABC",
        description: "Seguimiento de cotización pendiente",
        priority: "high",
        dueDate: "2024-01-16",
        completed: false,
        client: "Constructora ABC"
    },
    {
        id: 2,
        title: "Preparar presentación para Desarrollos XYZ",
        description: "Incluir nuevos productos y precios especiales",
        priority: "medium",
        dueDate: "2024-01-18",
        completed: false,
        client: "Desarrollos XYZ"
    },
    {
        id: 3,
        title: "Enviar muestras a Inmobiliaria DEF",
        description: "Muestras de materiales solicitadas",
        priority: "low",
        dueDate: "2024-01-20",
        completed: true,
        client: "Inmobiliaria DEF"
    },
    {
        id: 4,
        title: "Revisar contrato con Grupo Constructor GHI",
        description: "Verificar términos y condiciones",
        priority: "high",
        dueDate: "2024-01-14",
        completed: false,
        client: "Grupo Constructor GHI"
    },
    {
        id: 5,
        title: "Actualizar base de datos de productos",
        description: "Incluir nuevos precios y disponibilidad",
        priority: "medium",
        dueDate: "2024-01-19",
        completed: false,
        client: null
    }
];

// Visits data with document attachments
const visitsData = [
    {
        id: 1,
        clientId: 1,
        clientName: "Constructora ABC",
        contactName: "María González",
        type: "presencial",
        vendor: "Juan Pérez",
        dateTime: "2024-01-15 14:30",
        status: "completada",
        communication: "Se discutieron los requerimientos del nuevo proyecto de vivienda. Cliente interesado en materiales premium.",
        notes: "Cliente muy receptivo. Solicitar cotización para 500 m2 de material. Programar seguimiento en 3 días.",
        documents: ["cotizacion_v1.pdf", "planos_proyecto.jpg"]
    },
    {
        id: 2,
        clientId: 2,
        clientName: "Desarrollos XYZ",
        contactName: "Carlos Rodríguez",
        type: "virtual",
        vendor: "María García",
        dateTime: "2024-01-14 10:00",
        status: "completada",
        communication: "Videoconferencia para revisar propuesta técnica. Se enviaron especificaciones por email.",
        notes: "Cliente satisfecho con la propuesta. Pendiente aprobación del comité directivo.",
        documents: ["propuesta_tecnica.pdf"]
    },
    {
        id: 3,
        clientId: 3,
        clientName: "Inmobiliaria DEF",
        contactName: "Ana Martínez",
        type: "llamada",
        vendor: "Carlos Ruiz",
        dateTime: "2024-01-13 16:15",
        status: "completada",
        communication: "Llamada de seguimiento post-cotización. Cliente solicita ajustes en precios.",
        notes: "Negociar descuento del 8%. Cliente tiene presupuesto limitado pero proyecto seguro.",
        documents: []
    },
    {
        id: 4,
        clientId: 1,
        clientName: "Constructora ABC",
        contactName: "Luis Hernández",
        type: "email",
        vendor: "Juan Pérez",
        dateTime: "2024-01-12 09:30",
        status: "completada",
        communication: "Intercambio de emails sobre especificaciones técnicas y tiempos de entrega.",
        notes: "Cliente requiere entrega urgente. Verificar disponibilidad en almacén.",
        documents: ["especificaciones.pdf"]
    },
    {
        id: 5,
        clientId: 4,
        clientName: "Grupo Constructor GHI",
        contactName: "Patricia López",
        type: "whatsapp",
        vendor: "Ana López",
        dateTime: "2024-01-11 18:45",
        status: "pendiente",
        communication: "Mensajes sobre modificaciones al pedido original.",
        notes: "Cliente solicita cambio de materiales. Programar reunión presencial.",
        documents: ["modificaciones.jpg"]
    }
];

// Enhanced quotations data with price history
const quotationsData = [
    {
        id: "COT-2024-001",
        clientId: 1,
        clientName: "Constructora ABC",
        vendor: "Juan Pérez",
        date: "2024-01-15",
        validUntil: "2024-01-30",
        quotedValue: 187500,
        finalPrice: 168750,
        discount: 10,
        status: "cerrada",
        products: [
            { name: "Cemento Portland", quantity: 100, unitPrice: 285.50, total: 28550 },
            { name: "Varilla de Acero #4", quantity: 500, unitPrice: 12.75, total: 6375 },
            { name: "Ladrillo Rojo", quantity: 10000, unitPrice: 3.25, total: 32500 }
        ],
        discountPolicy: "15-20%",
        notes: "Descuento por volumen aplicado según política del cliente"
    },
    {
        id: "COT-2024-002",
        clientId: 2,
        clientName: "Desarrollos XYZ",
        vendor: "María García",
        date: "2024-01-14",
        validUntil: "2024-01-28",
        quotedValue: 156000,
        finalPrice: 140400,
        discount: 10,
        status: "en_proceso",
        products: [
            { name: "Mezcladora de Concreto", quantity: 2, unitPrice: 8500, total: 17000 },
            { name: "Taladro Industrial", quantity: 5, unitPrice: 1250, total: 6250 }
        ],
        discountPolicy: "10-12%",
        notes: "Cliente preferencial con historial de pagos puntuales"
    },
    {
        id: "COT-2024-003",
        clientId: 3,
        clientName: "Inmobiliaria DEF",
        vendor: "Carlos Ruiz",
        date: "2024-01-13",
        validUntil: "2024-01-27",
        quotedValue: 89750,
        finalPrice: null,
        discount: 8,
        status: "pendiente",
        products: [
            { name: "Pintura Vinílica", quantity: 50, unitPrice: 185, total: 9250 },
            { name: "Cemento Portland", quantity: 50, unitPrice: 285.50, total: 14275 }
        ],
        discountPolicy: "8-10%",
        notes: "Descuento mínimo por ser cliente nuevo"
    }
];

const eventsData = [
    {
        id: 1,
        title: "Reunión con Constructora ABC",
        type: "meeting",
        date: "2024-01-16",
        time: "10:00",
        duration: 60,
        client: "Constructora ABC"
    },
    {
        id: 2,
        title: "Llamada de seguimiento",
        type: "call",
        date: "2024-01-17",
        time: "14:30",
        duration: 30,
        client: "Desarrollos XYZ"
    },
    {
        id: 3,
        title: "Presentación de productos",
        type: "presentation",
        date: "2024-01-18",
        time: "11:00",
        duration: 90,
        client: "Inmobiliaria DEF"
    },
    {
        id: 4,
        title: "Seguimiento post-venta",
        type: "follow-up",
        date: "2024-01-19",
        time: "15:00",
        duration: 45,
        client: "Grupo Constructor GHI"
    }
];

const productsData = [
    {
        id: 1,
        name: "Cemento Portland",
        category: "materials",
        description: "Cemento de alta calidad para construcción",
        price: 285.50,
        stock: 150
    },
    {
        id: 2,
        name: "Varilla de Acero #4",
        category: "materials",
        description: "Varilla corrugada de 3/8 pulgadas",
        price: 12.75,
        stock: 500
    },
    {
        id: 3,
        name: "Taladro Industrial",
        category: "tools",
        description: "Taladro de percusión profesional",
        price: 1250.00,
        stock: 25
    },
    {
        id: 4,
        name: "Mezcladora de Concreto",
        category: "equipment",
        description: "Mezcladora industrial 350L",
        price: 8500.00,
        stock: 8
    },
    {
        id: 5,
        name: "Ladrillo Rojo",
        category: "construction",
        description: "Ladrillo de arcilla cocida estándar",
        price: 3.25,
        stock: 10000
    },
    {
        id: 6,
        name: "Pintura Vinílica",
        category: "materials",
        description: "Pintura lavable para interiores",
        price: 185.00,
        stock: 75
    }
];

const notificationsData = [
    {
        id: 1,
        title: "Cotización próxima a vencer",
        message: "La cotización para Desarrollos XYZ vence mañana",
        type: "warning",
        time: "Hace 2 horas",
        unread: true
    },
    {
        id: 2,
        title: "Nuevo lead registrado",
        message: "Pedro Ramírez se registró desde el sitio web",
        type: "info",
        time: "Hace 4 horas",
        unread: true
    },
    {
        id: 3,
        title: "Tarea vencida",
        message: "Revisar contrato con Grupo Constructor GHI",
        type: "danger",
        time: "Hace 1 día",
        unread: false
    }
];

// Global variables
let currentSection = 'dashboard';
let charts = {};

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    initializeNavigation();
    initializeCharts();
    renderDashboard();
    renderClientCards();
    renderLeadsTable();
    renderTasksList();
    renderCalendar();
    renderActivityFeed();
    renderProductsGrid();
    renderNotificationsList();
    initializeChatbot();
    
    // Initialize mobile sidebar
    initializeMobileSidebar();
});

// Navigation functionality
function initializeNavigation() {
    const navLinks = document.querySelectorAll('.sidebar-nav .nav-link');
    
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Remove active class from all links
            navLinks.forEach(l => l.classList.remove('active'));
            
            // Add active class to clicked link
            this.classList.add('active');
            
            // Get section name
            const section = this.getAttribute('data-section');
            
            // Show corresponding section
            showSection(section);
        });
    });
}

function showSection(sectionName) {
    // Hide all sections
    const sections = document.querySelectorAll('.content-section');
    sections.forEach(section => {
        section.classList.remove('active');
    });
    
    // Show target section
    const targetSection = document.getElementById(sectionName);
    if (targetSection) {
        targetSection.classList.add('active');
        
        // Update page title
        updatePageTitle(sectionName);
        
        // Update current section
        currentSection = sectionName;
        
        // Trigger section-specific updates
        onSectionChange(sectionName);
    }
}

function updatePageTitle(sectionName) {
    const titles = {
        'dashboard': 'Dashboard',
        'portfolio': 'Portafolio de Clientes',
        'leads': 'Gestión de Leads',
        'tasks': 'Gestión de Tareas',
        'calendar': 'Calendario',
        'activity': 'Feed de Actividad',
        'reports': 'Reportes y Analytics',
        'products': 'Catálogo de Productos',
        'notifications': 'Centro de Notificaciones',
        'ai-assistant': 'Asistente de IA'
    };
    
    const pageTitle = document.getElementById('pageTitle');
    if (pageTitle && titles[sectionName]) {
        pageTitle.textContent = titles[sectionName];
    }
}

function onSectionChange(sectionName) {
    // Trigger specific actions when sections change
    switch(sectionName) {
        case 'dashboard':
            updateCharts();
            break;
        case 'calendar':
            renderCalendar();
            break;
        case 'reports':
            renderReportsCharts();
            break;
    }
}

// Mobile sidebar functionality
function initializeMobileSidebar() {
    const sidebarToggleMobile = document.getElementById('sidebarToggleMobile');
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    // Mobile toggle button
    if (sidebarToggleMobile) {
        sidebarToggleMobile.addEventListener('click', function() {
            toggleSidebar();
        });
    }
    
    // Sidebar close button
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', function() {
            closeSidebar();
        });
    }
    
    // Overlay click to close
    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', function() {
            closeSidebar();
        });
    }
    
    // Close sidebar when clicking nav links on mobile
    const navLinks = document.querySelectorAll('.sidebar-nav .nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', function() {
            if (window.innerWidth <= 991.98) {
                closeSidebar();
            }
        });
    });
}

function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    if (sidebar && sidebarOverlay) {
        sidebar.classList.toggle('show');
        sidebarOverlay.classList.toggle('show');
    }
}

function closeSidebar() {
    const sidebar = document.getElementById('sidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    if (sidebar && sidebarOverlay) {
        sidebar.classList.remove('show');
        sidebarOverlay.classList.remove('show');
    }
}

// Charts initialization
function initializeCharts() {
    initializeSalesFunnelChart();
    initializeStageDistributionChart();
    initializeLeadScoreChart();
    initializeTaskPriorityChart();
    initializeSalesTrendChart();
}

function initializeSalesFunnelChart() {
    const ctx = document.getElementById('salesFunnelChart');
    if (!ctx) return;
    
    // Destroy existing chart if it exists
    if (charts.salesFunnel) {
        charts.salesFunnel.destroy();
    }
    
    charts.salesFunnel = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Leads', 'Contactados', 'Calificados', 'Propuestas', 'Cerrados'],
            datasets: [{
                label: 'Cantidad',
                data: [120, 85, 65, 45, 28],
                backgroundColor: [
                    '#e3f2fd',
                    '#bbdefb',
                    '#90caf9',
                    '#64b5f6',
                    '#42a5f5'
                ],
                borderColor: [
                    '#1976d2',
                    '#1976d2',
                    '#1976d2',
                    '#1976d2',
                    '#1976d2'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                intersect: false
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.parsed.y + ' clientes';
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    }
                },
                y: {
                    beginAtZero: true,
                    max: 150,
                    ticks: {
                        stepSize: 25
                    },
                    grid: {
                        color: 'rgba(0,0,0,0.1)'
                    }
                }
            },
            layout: {
                padding: {
                    top: 10,
                    bottom: 10
                }
            }
        }
    });
}

function initializeStageDistributionChart() {
    const ctx = document.getElementById('stageDistributionChart');
    if (!ctx) return;
    
    // Destroy existing chart if it exists
    if (charts.stageDistribution) {
        charts.stageDistribution.destroy();
    }
    
    charts.stageDistribution = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Prospecto', 'Contactado', 'Cotizado', 'Cerrado'],
            datasets: [{
                data: [30, 25, 35, 10],
                backgroundColor: [
                    '#e3f2fd',
                    '#f3e5f5',
                    '#fff3e0',
                    '#e8f5e8'
                ],
                borderColor: [
                    '#1976d2',
                    '#7b1fa2',
                    '#f57c00',
                    '#388e3c'
                ],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                intersect: false
            },
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 15,
                        usePointStyle: true
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = ((context.parsed / total) * 100).toFixed(1);
                            return context.label + ': ' + context.parsed + ' (' + percentage + '%)';
                        }
                    }
                }
            },
            layout: {
                padding: {
                    top: 10,
                    bottom: 10
                }
            }
        }
    });
}

function initializeLeadScoreChart() {
    const ctx = document.getElementById('leadScoreChart');
    if (!ctx) return;
    
    charts.leadScore = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Alto (80-100)', 'Medio (50-79)', 'Bajo (0-49)'],
            datasets: [{
                data: [2, 2, 1],
                backgroundColor: ['#d4edda', '#fff3cd', '#f8d7da'],
                borderColor: ['#155724', '#856404', '#721c24'],
                borderWidth: 2
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

function initializeTaskPriorityChart() {
    const ctx = document.getElementById('taskPriorityChart');
    if (!ctx) return;
    
    charts.taskPriority = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Alta', 'Media', 'Baja'],
            datasets: [{
                label: 'Tareas',
                data: [2, 2, 1],
                backgroundColor: ['#f8d7da', '#fff3cd', '#d4edda'],
                borderColor: ['#dc3545', '#ffc107', '#198754'],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            }
        }
    });
}

function initializeSalesTrendChart() {
    const ctx = document.getElementById('salesTrendChart');
    if (!ctx) return;
    
    charts.salesTrend = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Ago', 'Sep', 'Oct', 'Nov', 'Dic', 'Ene'],
            datasets: [{
                label: 'Ventas ($)',
                data: [285000, 310000, 295000, 340000, 325000, 342580],
                borderColor: '#0d6efd',
                backgroundColor: 'rgba(13, 110, 253, 0.1)',
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: false,
                    ticks: {
                        callback: function(value) {
                            return '$' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

function updateCharts() {
    Object.values(charts).forEach(chart => {
        if (chart && typeof chart.update === 'function') {
            chart.update();
        }
    });
}

function renderReportsCharts() {
    // This function can be expanded to render additional charts for the reports section
    updateCharts();
}

// Dashboard rendering
function renderDashboard() {
    // Dashboard is mostly static HTML, but we can add dynamic updates here
    console.log('Dashboard rendered');
}

// Client cards rendering
function renderClientCards() {
    const container = document.getElementById('clientCards');
    if (!container) return;
    
    container.innerHTML = '';
    
    clientsData.forEach(client => {
        const card = createClientCard(client);
        container.appendChild(card);
    });
}

function createClientCard(client) {
    const col = document.createElement('div');
    col.className = 'col-lg-4 col-md-6 mb-4';
    
    const avatarColor = getAvatarColor(client.id);
    const initials = client.contact.split(' ').map(n => n[0]).join('');
    
    col.innerHTML = `
        <div class="card client-card h-100" onclick="showClientDetails(${client.id})">
            <div class="card-body text-center">
                <div class="client-avatar mx-auto" style="background: ${avatarColor}">
                    ${initials}
                </div>
                <h5 class="card-title">${client.name}</h5>
                <p class="text-muted mb-2">${client.contact}</p>
                <span class="status-badge status-${client.status}">${getStatusText(client.status)}</span>
                <hr>
                <div class="row text-center">
                    <div class="col-6">
                        <small class="text-muted">Valor</small>
                        <div class="fw-bold text-success">$${client.value.toLocaleString()}</div>
                    </div>
                    <div class="col-6">
                        <small class="text-muted">Último Contacto</small>
                        <div class="fw-bold">${formatDate(client.lastContact)}</div>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    return col;
}

function getAvatarColor(id) {
    const colors = [
        'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        'linear-gradient(135deg, #f093fb 0%, #f5576c 100%)',
        'linear-gradient(135deg, #4facfe 0%, #00f2fe 100%)',
        'linear-gradient(135deg, #43e97b 0%, #38f9d7 100%)',
        'linear-gradient(135deg, #fa709a 0%, #fee140 100%)',
        'linear-gradient(135deg, #a8edea 0%, #fed6e3 100%)'
    ];
    return colors[id % colors.length];
}

function getStatusText(status) {
    const statusTexts = {
        'prospecto': 'Prospecto',
        'contactado': 'Contactado',
        'cotizado': 'Cotizado',
        'cerrado': 'Cerrado'
    };
    return statusTexts[status] || status;
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', { 
        day: '2-digit', 
        month: '2-digit' 
    });
}

function showClientDetails(clientId) {
    const client = clientsData.find(c => c.id === clientId);
    if (client) {
        alert(`Detalles del cliente:\n\nEmpresa: ${client.name}\nContacto: ${client.contact}\nTeléfono: ${client.phone}\nEmail: ${client.email}\nEstado: ${getStatusText(client.status)}\nValor: $${client.value.toLocaleString()}`);
    }
}

// Leads management
function renderLeadsTable() {
    const tbody = document.getElementById('leadsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    leadsData.forEach(lead => {
        const row = createLeadRow(lead);
        tbody.appendChild(row);
    });
}

function createLeadRow(lead) {
    const tr = document.createElement('tr');
    
    const scoreClass = lead.score >= 80 ? 'score-high' : lead.score >= 50 ? 'score-medium' : 'score-low';
    
    tr.innerHTML = `
        <td>${lead.name}</td>
        <td>${lead.company}</td>
        <td>${lead.email}</td>
        <td>${getSourceText(lead.source)}</td>
        <td><span class="lead-score ${scoreClass}">${lead.score}</span></td>
        <td><span class="lead-status status-${lead.status}">${getLeadStatusText(lead.status)}</span></td>
        <td>
            <button class="btn btn-sm btn-outline-primary me-1" onclick="editLead(${lead.id})">
                <i class="fas fa-edit"></i>
            </button>
            <button class="btn btn-sm btn-outline-success" onclick="convertLead(${lead.id})">
                <i class="fas fa-user-check"></i>
            </button>
        </td>
    `;
    
    return tr;
}

function getSourceText(source) {
    const sourceTexts = {
        'website': 'Sitio Web',
        'referral': 'Referencia',
        'social': 'Redes Sociales',
        'email': 'Email Marketing',
        'cold-call': 'Llamada Fría'
    };
    return sourceTexts[source] || source;
}

function getLeadStatusText(status) {
    const statusTexts = {
        'new': 'Nuevo',
        'contacted': 'Contactado',
        'qualified': 'Calificado',
        'converted': 'Convertido'
    };
    return statusTexts[status] || status;
}

function editLead(leadId) {
    const lead = leadsData.find(l => l.id === leadId);
    if (lead) {
        alert(`Editando lead: ${lead.name} - ${lead.company}`);
    }
}

function convertLead(leadId) {
    const lead = leadsData.find(l => l.id === leadId);
    if (lead) {
        lead.status = 'converted';
        renderLeadsTable();
        showToast('Lead convertido exitosamente', 'success');
    }
}

// Tasks management
function renderTasksList() {
    const container = document.getElementById('tasksContainer');
    if (!container) return;
    
    container.innerHTML = '';
    
    tasksData.forEach(task => {
        const taskElement = createTaskElement(task);
        container.appendChild(taskElement);
    });
    
    updateTaskCounts();
}

function createTaskElement(task) {
    const div = document.createElement('div');
    div.className = `task-item ${task.completed ? 'completed' : ''} ${isTaskOverdue(task) ? 'overdue' : ''}`;
    
    const priorityClass = `priority-${task.priority}`;
    const isOverdue = isTaskOverdue(task);
    
    div.innerHTML = `
        <div class="task-priority ${priorityClass}"></div>
        <div class="d-flex align-items-start">
            <div class="form-check me-3">
                <input class="form-check-input" type="checkbox" ${task.completed ? 'checked' : ''} 
                       onchange="toggleTask(${task.id})">
            </div>
            <div class="flex-grow-1">
                <div class="task-title">${task.title}</div>
                <div class="task-description text-muted">${task.description}</div>
                <div class="task-meta">
                    <span class="badge bg-${getPriorityColor(task.priority)} me-2">${getPriorityText(task.priority)}</span>
                    <span class="${isOverdue ? 'task-overdue' : ''}">
                        <i class="fas fa-calendar me-1"></i>${formatDate(task.dueDate)}
                    </span>
                    ${task.client ? `<span class="ms-2"><i class="fas fa-user me-1"></i>${task.client}</span>` : ''}
                </div>
            </div>
            <div class="dropdown">
                <button class="btn btn-sm btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                    <i class="fas fa-ellipsis-v"></i>
                </button>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="#" onclick="editTask(${task.id})">
                        <i class="fas fa-edit me-2"></i>Editar
                    </a></li>
                    <li><a class="dropdown-item text-danger" href="#" onclick="deleteTask(${task.id})">
                        <i class="fas fa-trash me-2"></i>Eliminar
                    </a></li>
                </ul>
            </div>
        </div>
    `;
    
    return div;
}

function getPriorityColor(priority) {
    const colors = {
        'high': 'danger',
        'medium': 'warning',
        'low': 'success'
    };
    return colors[priority] || 'secondary';
}

function getPriorityText(priority) {
    const texts = {
        'high': 'Alta',
        'medium': 'Media',
        'low': 'Baja'
    };
    return texts[priority] || priority;
}

function isTaskOverdue(task) {
    if (task.completed) return false;
    const today = new Date();
    const dueDate = new Date(task.dueDate);
    return dueDate < today;
}

function toggleTask(taskId) {
    const task = tasksData.find(t => t.id === taskId);
    if (task) {
        task.completed = !task.completed;
        renderTasksList();
        showToast(task.completed ? 'Tarea completada' : 'Tarea marcada como pendiente', 'success');
    }
}

function updateTaskCounts() {
    const pendingCount = tasksData.filter(t => !t.completed).length;
    const completedCount = tasksData.filter(t => t.completed).length;
    const overdueCount = tasksData.filter(t => !t.completed && isTaskOverdue(t)).length;
    
    const pendingElement = document.getElementById('pendingTasksCount');
    const completedElement = document.getElementById('completedTasksCount');
    const overdueElement = document.getElementById('overdueTasksCount');
    
    if (pendingElement) pendingElement.textContent = pendingCount;
    if (completedElement) completedElement.textContent = completedCount;
    if (overdueElement) overdueElement.textContent = overdueCount;
}

function editTask(taskId) {
    const task = tasksData.find(t => t.id === taskId);
    if (task) {
        alert(`Editando tarea: ${task.title}`);
    }
}

function deleteTask(taskId) {
    if (confirm('¿Estás seguro de que quieres eliminar esta tarea?')) {
        const index = tasksData.findIndex(t => t.id === taskId);
        if (index > -1) {
            tasksData.splice(index, 1);
            renderTasksList();
            showToast('Tarea eliminada', 'success');
        }
    }
}

// Calendar functionality
function renderCalendar() {
    const container = document.getElementById('calendarGrid');
    if (!container) return;
    
    // Simple calendar implementation
    container.innerHTML = `
        <div class="text-center p-4">
            <i class="fas fa-calendar-alt fa-3x text-muted mb-3"></i>
            <h5>Vista de Calendario</h5>
            <p class="text-muted">Funcionalidad de calendario en desarrollo</p>
        </div>
    `;
    
    renderUpcomingEvents();
}

function renderUpcomingEvents() {
    const container = document.getElementById('upcomingEvents');
    if (!container) return;
    
    container.innerHTML = '';
    
    eventsData.forEach(event => {
        const eventElement = createEventElement(event);
        container.appendChild(eventElement);
    });
}

function createEventElement(event) {
    const div = document.createElement('div');
    div.className = 'mb-3 p-2 border-start border-3 border-primary';
    
    const typeIcon = getEventTypeIcon(event.type);
    const typeColor = getEventTypeColor(event.type);
    
    div.innerHTML = `
        <div class="d-flex align-items-center mb-1">
            <i class="${typeIcon} text-${typeColor} me-2"></i>
            <strong>${event.title}</strong>
        </div>
        <div class="text-muted small">
            <i class="fas fa-calendar me-1"></i>${formatDate(event.date)}
            <i class="fas fa-clock ms-2 me-1"></i>${event.time}
        </div>
        ${event.client ? `<div class="text-muted small mt-1">
            <i class="fas fa-user me-1"></i>${event.client}
        </div>` : ''}
    `;
    
    return div;
}

function getEventTypeIcon(type) {
    const icons = {
        'meeting': 'fas fa-handshake',
        'call': 'fas fa-phone',
        'presentation': 'fas fa-presentation',
        'follow-up': 'fas fa-calendar-check'
    };
    return icons[type] || 'fas fa-calendar';
}

function getEventTypeColor(type) {
    const colors = {
        'meeting': 'primary',
        'call': 'success',
        'presentation': 'info',
        'follow-up': 'warning'
    };
    return colors[type] || 'secondary';
}

// Activity feed
function renderActivityFeed() {
    const container = document.getElementById('activityFeed');
    if (!container) return;
    
    container.innerHTML = '';
    
    activitiesData.forEach(activity => {
        const activityElement = createActivityElement(activity);
        container.appendChild(activityElement);
    });
}

function createActivityElement(activity) {
    const div = document.createElement('div');
    div.className = 'activity-item';
    
    const typeIcon = getActivityTypeIcon(activity.type);
    const typeColor = getActivityTypeColor(activity.type);
    
    div.innerHTML = `
        <div class="activity-avatar" style="background-color: var(--${typeColor}-color)">
            <i class="${typeIcon}"></i>
        </div>
        <div class="activity-content">
            <div class="activity-title">${getActivityTypeText(activity.type)} - ${activity.client}</div>
            <div class="activity-description">${activity.description}</div>
            <div class="activity-time">
                <i class="fas fa-clock me-1"></i>${activity.timestamp} por ${activity.user}
            </div>
        </div>
    `;
    
    return div;
}

function getActivityTypeIcon(type) {
    const icons = {
        'call': 'fas fa-phone',
        'meeting': 'fas fa-handshake',
        'quote': 'fas fa-file-invoice',
        'follow-up': 'fas fa-calendar-check'
    };
    return icons[type] || 'fas fa-circle';
}

function getActivityTypeColor(type) {
    const colors = {
        'call': 'primary',
        'meeting': 'success',
        'quote': 'warning',
        'follow-up': 'info'
    };
    return colors[type] || 'secondary';
}

function getActivityTypeText(type) {
    const texts = {
        'call': 'Llamada',
        'meeting': 'Reunión',
        'quote': 'Cotización',
        'follow-up': 'Seguimiento'
    };
    return texts[type] || type;
}

// Products grid
function renderProductsGrid() {
    const container = document.getElementById('productsGrid');
    if (!container) return;
    
    container.innerHTML = '';
    
    productsData.forEach(product => {
        const productElement = createProductCard(product);
        container.appendChild(productElement);
    });
}

function createProductCard(product) {
    const col = document.createElement('div');
    col.className = 'col-lg-4 col-md-6 mb-4';
    
    const categoryIcon = getCategoryIcon(product.category);
    const categoryClass = `category-${product.category}`;
    
    col.innerHTML = `
        <div class="product-card">
            <div class="product-icon ${categoryClass}">
                <i class="${categoryIcon}"></i>
            </div>
            <div class="product-name">${product.name}</div>
            <div class="product-category">${getCategoryText(product.category)}</div>
            <div class="product-description">${product.description}</div>
            <div class="product-price">$${product.price.toFixed(2)}</div>
            <div class="product-stock">Stock: ${product.stock}</div>
            <div class="d-flex gap-2">
                <button class="btn btn-primary btn-sm flex-fill" onclick="quoteProduct(${product.id})">
                    <i class="fas fa-calculator me-1"></i>Cotizar
                </button>
                <button class="btn btn-outline-secondary btn-sm" onclick="editProduct(${product.id})">
                    <i class="fas fa-edit"></i>
                </button>
            </div>
        </div>
    `;
    
    return col;
}

function getCategoryIcon(category) {
    const icons = {
        'construction': 'fas fa-building',
        'tools': 'fas fa-tools',
        'materials': 'fas fa-cubes',
        'equipment': 'fas fa-cogs'
    };
    return icons[category] || 'fas fa-box';
}

function getCategoryText(category) {
    const texts = {
        'construction': 'Construcción',
        'tools': 'Herramientas',
        'materials': 'Materiales',
        'equipment': 'Equipos'
    };
    return texts[category] || category;
}

function quoteProduct(productId) {
    const product = productsData.find(p => p.id === productId);
    if (product) {
        alert(`Cotizando: ${product.name}\nPrecio: $${product.price.toFixed(2)}`);
    }
}

function editProduct(productId) {
    const product = productsData.find(p => p.id === productId);
    if (product) {
        alert(`Editando producto: ${product.name}`);
    }
}

// Notifications
function renderNotificationsList() {
    const container = document.getElementById('notificationsList');
    if (!container) return;
    
    container.innerHTML = '';
    
    notificationsData.forEach(notification => {
        const notificationElement = createNotificationElement(notification);
        container.appendChild(notificationElement);
    });
}

function createNotificationElement(notification) {
    const div = document.createElement('div');
    div.className = `notification-item ${notification.unread ? 'unread' : ''}`;
    
    const typeIcon = getNotificationTypeIcon(notification.type);
    const typeColor = getNotificationTypeColor(notification.type);
    
    div.innerHTML = `
        <div class="notification-icon" style="background-color: var(--${typeColor}-color)">
            <i class="${typeIcon} text-white"></i>
        </div>
        <div class="notification-content">
            <div class="notification-title">${notification.title}</div>
            <div class="notification-message">${notification.message}</div>
            <div class="notification-time">${notification.time}</div>
        </div>
    `;
    
    return div;
}

function getNotificationTypeIcon(type) {
    const icons = {
        'warning': 'fas fa-exclamation-triangle',
        'info': 'fas fa-info-circle',
        'danger': 'fas fa-times-circle',
        'success': 'fas fa-check-circle'
    };
    return icons[type] || 'fas fa-bell';
}

function getNotificationTypeColor(type) {
    const colors = {
        'warning': 'warning',
        'info': 'info',
        'danger': 'danger',
        'success': 'success'
    };
    return colors[type] || 'primary';
}

// Chatbot functionality
function initializeChatbot() {
    const chatInput = document.getElementById('chatInput');
    if (chatInput) {
        chatInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });
    }
}

function sendMessage() {
    const input = document.getElementById('chatInput');
    const message = input.value.trim();
    
    if (message) {
        addMessageToChat(message, 'user');
        input.value = '';
        
        // Simulate AI response
        setTimeout(() => {
            const response = generateAIResponse(message);
            addMessageToChat(response, 'assistant');
        }, 1000);
    }
}

function sendQuickMessage(message) {
    addMessageToChat(message, 'user');
    
    setTimeout(() => {
        const response = generateAIResponse(message);
        addMessageToChat(response, 'assistant');
    }, 1000);
}

function addMessageToChat(message, sender) {
    const container = document.getElementById('chatContainer');
    if (!container) return;
    
    const messageDiv = document.createElement('div');
    messageDiv.className = `chat-message ${sender}-message`;
    
    const time = new Date().toLocaleTimeString('es-ES', { 
        hour: '2-digit', 
        minute: '2-digit' 
    });
    
    if (sender === 'user') {
        messageDiv.innerHTML = `
            <div class="message-content">
                <p>${message}</p>
                <small class="text-muted">${time}</small>
            </div>
            <div class="message-avatar">
                <i class="fas fa-user"></i>
            </div>
        `;
    } else {
        messageDiv.innerHTML = `
            <div class="message-avatar">
                <i class="fas fa-robot"></i>
            </div>
            <div class="message-content">
                <p>${message}</p>
                <small class="text-muted">${time}</small>
            </div>
        `;
    }
    
    container.appendChild(messageDiv);
    container.scrollTop = container.scrollHeight;
}

function generateAIResponse(message) {
    const responses = {
        'tareas pendientes': 'Tienes 2 tareas de alta prioridad pendientes: "Llamar a Constructora ABC" y "Revisar contrato con Grupo Constructor GHI".',
        'seguimiento': 'Los siguientes clientes necesitan seguimiento: Inmobiliaria DEF (sin contactar por 6 días) y Edificaciones MNO (sin contactar por 5 días).',
        'rendimiento': 'Este mes has cerrado $342,580 en ventas, un 12% más que el mes anterior. ¡Excelente trabajo!',
        'default': 'Entiendo tu consulta. ¿Podrías ser más específico sobre qué información necesitas? Puedo ayudarte con tareas, clientes, ventas y más.'
    };
    
    const lowerMessage = message.toLowerCase();
    
    if (lowerMessage.includes('tarea')) return responses['tareas pendientes'];
    if (lowerMessage.includes('seguimiento') || lowerMessage.includes('cliente')) return responses['seguimiento'];
    if (lowerMessage.includes('rendimiento') || lowerMessage.includes('venta')) return responses['rendimiento'];
    
    return responses['default'];
}

// Modal functions
function showAddLeadModal() {
    const modal = new bootstrap.Modal(document.getElementById('addLeadModal'));
    modal.show();
}

function showAddTaskModal() {
    const modal = new bootstrap.Modal(document.getElementById('addTaskModal'));
    modal.show();
}

function showAddEventModal() {
    const modal = new bootstrap.Modal(document.getElementById('addEventModal'));
    modal.show();
}

function showAddProductModal() {
    const modal = new bootstrap.Modal(document.getElementById('addProductModal'));
    modal.show();
}

function saveLead() {
    // Get form data
    const name = document.getElementById('leadName').value;
    const company = document.getElementById('leadCompany').value;
    const email = document.getElementById('leadEmail').value;
    const phone = document.getElementById('leadPhone').value;
    const source = document.getElementById('leadSource').value;
    
    if (name && company && email && source) {
        // Add to leads data
        const newLead = {
            id: leadsData.length + 1,
            name: name,
            company: company,
            email: email,
            phone: phone,
            source: source,
            score: Math.floor(Math.random() * 100),
            status: 'new'
        };
        
        leadsData.push(newLead);
        renderLeadsTable();
        
        // Close modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('addLeadModal'));
        modal.hide();
        
        // Reset form
        document.getElementById('addLeadForm').reset();
        
        showToast('Lead agregado exitosamente', 'success');
    }
}

function saveTask() {
    // Get form data
    const title = document.getElementById('taskTitle').value;
    const description = document.getElementById('taskDescription').value;
    const priority = document.getElementById('taskPriority').value;
    const dueDate = document.getElementById('taskDueDate').value;
    const client = document.getElementById('taskClient').value;
    
    if (title && dueDate) {
        // Add to tasks data
        const newTask = {
            id: tasksData.length + 1,
            title: title,
            description: description,
            priority: priority,
            dueDate: dueDate,
            completed: false,
            client: client || null
        };
        
        tasksData.push(newTask);
        renderTasksList();
        
        // Close modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('addTaskModal'));
        modal.hide();
        
        // Reset form
        document.getElementById('addTaskForm').reset();
        
        showToast('Tarea creada exitosamente', 'success');
    }
}

function saveEvent() {
    // Get form data
    const title = document.getElementById('eventTitle').value;
    const type = document.getElementById('eventType').value;
    const date = document.getElementById('eventDate').value;
    const time = document.getElementById('eventTime').value;
    const duration = document.getElementById('eventDuration').value;
    const client = document.getElementById('eventClient').value;
    
    if (title && type && date && time) {
        // Add to events data
        const newEvent = {
            id: eventsData.length + 1,
            title: title,
            type: type,
            date: date,
            time: time,
            duration: parseInt(duration),
            client: client || null
        };
        
        eventsData.push(newEvent);
        renderUpcomingEvents();
        
        // Close modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('addEventModal'));
        modal.hide();
        
        // Reset form
        document.getElementById('addEventForm').reset();
        
        showToast('Evento creado exitosamente', 'success');
    }
}

function saveProduct() {
    // Get form data
    const name = document.getElementById('productName').value;
    const category = document.getElementById('productCategory').value;
    const description = document.getElementById('productDescription').value;
    const price = parseFloat(document.getElementById('productPrice').value);
    const stock = parseInt(document.getElementById('productStock').value);
    
    if (name && category && price && stock) {
        // Add to products data
        const newProduct = {
            id: productsData.length + 1,
            name: name,
            category: category,
            description: description,
            price: price,
            stock: stock
        };
        
        productsData.push(newProduct);
        renderProductsGrid();
        
        // Close modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('addProductModal'));
        modal.hide();
        
        // Reset form
        document.getElementById('addProductForm').reset();
        
        showToast('Producto agregado exitosamente', 'success');
    }
}

// Utility functions
function showToast(message, type = 'info') {
    const toastContainer = document.getElementById('toastContainer');
    if (!toastContainer) return;
    
    const toastId = 'toast-' + Date.now();
    const toastHTML = `
        <div id="${toastId}" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <i class="fas fa-${getToastIcon(type)} text-${type} me-2"></i>
                <strong class="me-auto">CRM Demo</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        </div>
    `;
    
    toastContainer.insertAdjacentHTML('beforeend', toastHTML);
    
    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    
    // Remove toast element after it's hidden
    toastElement.addEventListener('hidden.bs.toast', function() {
        toastElement.remove();
    });
}

function getToastIcon(type) {
    const icons = {
        'success': 'check-circle',
        'warning': 'exclamation-triangle',
        'danger': 'times-circle',
        'info': 'info-circle'
    };
    return icons[type] || 'info-circle';
}

function exportReport() {
    showToast('Función de exportación en desarrollo', 'info');
}

// Calendar navigation functions
function changeCalendarView(view) {
    showToast(`Vista de calendario cambiada a: ${view}`, 'info');
}

function navigateCalendar(direction) {
    showToast(`Navegando calendario: ${direction}`, 'info');
}


// Enhanced CRM Analytics Data - using existing quotationsData
/*
const quotationsData = [
    {
        id: 'COT-2024-001',
        client: 'Constructora ABC',
        salesperson: 'Juan Pérez',
        date: '2024-01-15',
        value: 125430,
        status: 'closed',
        probability: 100,
        daysActive: 12,
        segment: 'enterprise'
    },
    {
        id: 'COT-2024-002',
        client: 'Desarrollos XYZ',
        salesperson: 'María García',
        date: '2024-01-14',
        value: 89750,
        status: 'in-process',
        probability: 85,
        daysActive: 8,
        segment: 'medium'
    },
    {
        id: 'COT-2024-003',
        client: 'Inmobiliaria DEF',
        salesperson: 'Carlos Ruiz',
        date: '2024-01-13',
        value: 67200,
        status: 'closed',
        probability: 100,
        daysActive: 15,
        segment: 'medium'
    },
    {
        id: 'COT-2024-004',
        client: 'Grupo Constructor GHI',
        salesperson: 'Juan Pérez',
        date: '2024-01-12',
        value: 156800,
        status: 'in-process',
        probability: 75,
        daysActive: 18,
        segment: 'enterprise'
    },
    {
        id: 'COT-2024-005',
        client: 'Proyectos JKL',
        salesperson: 'María García',
        date: '2024-01-11',
        value: 43500,
        status: 'lost',
        probability: 0,
        daysActive: 25,
        segment: 'small'
    },
    {
        id: 'COT-2024-006',
        client: 'Edificaciones MNO',
        salesperson: 'Carlos Ruiz',
        date: '2024-01-10',
        value: 98600,
        status: 'closed',
        probability: 100,
        daysActive: 22,
        segment: 'medium'
    },
    {
        id: 'COT-2024-007',
        client: 'Constructora PQR',
        salesperson: 'Juan Pérez',
        date: '2024-01-09',
        value: 187300,
        status: 'in-process',
        probability: 90,
        daysActive: 5,
        segment: 'enterprise'
    },
    {
        id: 'COT-2024-008',
        client: 'Desarrollos STU',
        salesperson: 'María García',
        date: '2024-01-08',
        value: 76400,
        status: 'closed',
        probability: 100,
        daysActive: 19,
        segment: 'medium'
    }
];
*/

const conversionData = {
    monthly: {
        labels: ['Ago 2023', 'Sep 2023', 'Oct 2023', 'Nov 2023', 'Dic 2023', 'Ene 2024'],
        quotations: [185, 198, 176, 203, 210, 249],
        closed: [112, 125, 108, 132, 131, 168],
        conversion: [60.5, 63.1, 61.4, 65.0, 62.4, 67.5]
    },
    weekly: {
        labels: ['Sem 1', 'Sem 2', 'Sem 3', 'Sem 4'],
        quotations: [58, 62, 67, 62],
        closed: [38, 42, 46, 42],
        conversion: [65.5, 67.7, 68.7, 67.7]
    },
    daily: {
        labels: ['Lun', 'Mar', 'Mié', 'Jue', 'Vie'],
        quotations: [12, 15, 18, 14, 16],
        closed: [8, 10, 12, 9, 11],
        conversion: [66.7, 66.7, 66.7, 64.3, 68.8]
    }
};

// Enhanced Charts Initialization
function renderReportsCharts() {
    initializeConversionAnalysisChart();
    initializeQuotationStatusChart();
    initializeSalesPersonPerformanceChart();
    initializeSegmentValueChart();
    initializeProductAnalysisChart();
    renderQuotationsTable();
}

function initializeConversionAnalysisChart() {
    const ctx = document.getElementById('conversionAnalysisChart');
    if (!ctx) return;
    
    const data = conversionData.monthly;
    
    if (charts.conversionAnalysis) {
        charts.conversionAnalysis.destroy();
    }
    
    charts.conversionAnalysis = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.labels,
            datasets: [
                {
                    label: 'Cotizaciones',
                    data: data.quotations,
                    borderColor: '#3b82f6',
                    backgroundColor: 'rgba(59, 130, 246, 0.1)',
                    yAxisID: 'y',
                    tension: 0.4
                },
                {
                    label: 'Cerradas',
                    data: data.closed,
                    borderColor: '#10b981',
                    backgroundColor: 'rgba(16, 185, 129, 0.1)',
                    yAxisID: 'y',
                    tension: 0.4
                },
                {
                    label: 'Conversión (%)',
                    data: data.conversion,
                    borderColor: '#f59e0b',
                    backgroundColor: 'rgba(245, 158, 11, 0.1)',
                    yAxisID: 'y1',
                    type: 'line',
                    tension: 0.4
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                mode: 'index',
                intersect: false,
            },
            scales: {
                y: {
                    type: 'linear',
                    display: true,
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Cantidad'
                    }
                },
                y1: {
                    type: 'linear',
                    display: true,
                    position: 'right',
                    title: {
                        display: true,
                        text: 'Conversión (%)'
                    },
                    grid: {
                        drawOnChartArea: false,
                    },
                }
            },
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    callbacks: {
                        afterLabel: function(context) {
                            if (context.datasetIndex === 2) {
                                return context.parsed.y + '%';
                            }
                            return '';
                        }
                    }
                }
            }
        }
    });
}

function initializeQuotationStatusChart() {
    const ctx = document.getElementById('quotationStatusChart');
    if (!ctx) return;
    
    // Destroy existing chart if it exists
    if (charts.quotationStatus) {
        charts.quotationStatus.destroy();
    }
    
    charts.quotationStatus = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Cerradas', 'En Proceso', 'Perdidas'],
            datasets: [{
                data: [142, 38, 30],
                backgroundColor: [
                    '#10b981',
                    '#f59e0b',
                    '#ef4444'
                ],
                borderColor: [
                    '#059669',
                    '#d97706',
                    '#dc2626'
                ],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                intersect: false
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            const total = context.dataset.data.reduce((a, b) => a + b, 0);
                            const percentage = ((context.parsed / total) * 100).toFixed(1);
                            return context.label + ': ' + context.parsed + ' (' + percentage + '%)';
                        }
                    }
                }
            },
            layout: {
                padding: {
                    top: 10,
                    bottom: 10
                }
            }
        }
    });
}

function initializeSalesPersonPerformanceChart() {
    const ctx = document.getElementById('salesPersonPerformanceChart');
    if (!ctx) return;
    
    if (charts.salesPersonPerformance) {
        charts.salesPersonPerformance.destroy();
    }
    
    charts.salesPersonPerformance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Juan Pérez', 'María García', 'Carlos Ruiz'],
            datasets: [
                {
                    label: 'Cotizaciones',
                    data: [89, 76, 84],
                    backgroundColor: 'rgba(59, 130, 246, 0.8)',
                    borderColor: '#3b82f6',
                    borderWidth: 1
                },
                {
                    label: 'Cerradas',
                    data: [62, 48, 58],
                    backgroundColor: 'rgba(16, 185, 129, 0.8)',
                    borderColor: '#10b981',
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'top',
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Cantidad'
                    }
                }
            }
        }
    });
}

function initializeSegmentValueChart() {
    const ctx = document.getElementById('segmentValueChart');
    if (!ctx) return;
    
    if (charts.segmentValue) {
        charts.segmentValue.destroy();
    }
    
    charts.segmentValue = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Empresarial', 'Mediano', 'Pequeño'],
            datasets: [{
                label: 'Valor Promedio ($)',
                data: [156800, 78400, 43500],
                backgroundColor: [
                    'rgba(139, 69, 19, 0.8)',
                    'rgba(255, 165, 0, 0.8)',
                    'rgba(50, 205, 50, 0.8)'
                ],
                borderColor: [
                    '#8b4513',
                    '#ffa500',
                    '#32cd32'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Valor ($)'
                    },
                    ticks: {
                        callback: function(value) {
                            return '$' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

function initializeProductAnalysisChart() {
    const ctx = document.getElementById('productAnalysisChart');
    if (!ctx) return;
    
    if (charts.productAnalysis) {
        charts.productAnalysis.destroy();
    }
    
    charts.productAnalysis = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Materiales', 'Herramientas', 'Equipos', 'Construcción'],
            datasets: [{
                data: [45, 25, 20, 10],
                backgroundColor: [
                    '#3b82f6',
                    '#10b981',
                    '#f59e0b',
                    '#ef4444'
                ],
                borderColor: [
                    '#1d4ed8',
                    '#059669',
                    '#d97706',
                    '#dc2626'
                ],
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 10,
                        usePointStyle: true
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.label + ': ' + context.parsed + '%';
                        }
                    }
                }
            }
        }
    });
}

function renderQuotationsTable() {
    const tbody = document.getElementById('quotationsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    quotationsData.forEach(quotation => {
        const row = createQuotationRow(quotation);
        tbody.appendChild(row);
    });
}

function createQuotationRow(quotation) {
    const tr = document.createElement('tr');
    
    const statusClass = getQuotationStatusClass(quotation.status);
    const statusText = getQuotationStatusText(quotation.status);
    
    tr.innerHTML = `
        <td><strong>${quotation.id}</strong></td>
        <td>${quotation.client}</td>
        <td>${quotation.salesperson}</td>
        <td>${formatDate(quotation.date)}</td>
        <td class="fw-bold text-success">$${quotation.value.toLocaleString()}</td>
        <td><span class="badge ${statusClass}">${statusText}</span></td>
        <td>
            <div class="d-flex align-items-center">
                <div class="progress flex-grow-1 me-2" style="height: 8px;">
                    <div class="progress-bar ${getProbabilityColor(quotation.probability)}" 
                         style="width: ${quotation.probability}%"></div>
                </div>
                <small class="text-muted">${quotation.probability}%</small>
            </div>
        </td>
        <td>
            <span class="${quotation.daysActive > 20 ? 'text-danger fw-bold' : 'text-muted'}">
                ${quotation.daysActive} días
            </span>
        </td>
        <td>
            <div class="btn-group btn-group-sm">
                <button class="btn btn-outline-primary" onclick="viewQuotation('${quotation.id}')">
                    <i class="fas fa-eye"></i>
                </button>
                <button class="btn btn-outline-success" onclick="editQuotation('${quotation.id}')">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-outline-info" onclick="duplicateQuotation('${quotation.id}')">
                    <i class="fas fa-copy"></i>
                </button>
            </div>
        </td>
    `;
    
    return tr;
}

function getQuotationStatusClass(status) {
    const classes = {
        'closed': 'bg-success',
        'in-process': 'bg-warning',
        'lost': 'bg-danger'
    };
    return classes[status] || 'bg-secondary';
}

function getQuotationStatusText(status) {
    const texts = {
        'closed': 'Cerrada',
        'in-process': 'En Proceso',
        'lost': 'Perdida'
    };
    return texts[status] || status;
}

function getProbabilityColor(probability) {
    if (probability >= 80) return 'bg-success';
    if (probability >= 60) return 'bg-info';
    if (probability >= 40) return 'bg-warning';
    return 'bg-danger';
}

// Report interaction functions
function changeConversionView(view) {
    const data = conversionData[view];
    if (data && charts.conversionAnalysis) {
        charts.conversionAnalysis.data.labels = data.labels;
        charts.conversionAnalysis.data.datasets[0].data = data.quotations;
        charts.conversionAnalysis.data.datasets[1].data = data.closed;
        charts.conversionAnalysis.data.datasets[2].data = data.conversion;
        charts.conversionAnalysis.update();
        
        // Update button states
        document.querySelectorAll('.btn-group .btn').forEach(btn => {
            btn.classList.remove('active');
        });
        event.target.classList.add('active');
    }
    
    showToast(`Vista cambiada a: ${view}`, 'info');
}

function refreshReports() {
    showToast('Actualizando reportes...', 'info');
    
    // Simulate data refresh
    setTimeout(() => {
        renderReportsCharts();
        showToast('Reportes actualizados exitosamente', 'success');
    }, 1500);
}

function viewQuotation(quotationId) {
    const quotation = quotationsData.find(q => q.id === quotationId);
    if (quotation) {
        alert(`Visualizando cotización: ${quotation.id}\n\nCliente: ${quotation.client}\nValor: $${quotation.value.toLocaleString()}\nEstado: ${getQuotationStatusText(quotation.status)}\nProbabilidad: ${quotation.probability}%`);
    }
}

function editQuotation(quotationId) {
    const quotation = quotationsData.find(q => q.id === quotationId);
    if (quotation) {
        showToast(`Editando cotización: ${quotation.id}`, 'info');
    }
}

function duplicateQuotation(quotationId) {
    const quotation = quotationsData.find(q => q.id === quotationId);
    if (quotation) {
        showToast(`Duplicando cotización: ${quotation.id}`, 'info');
    }
}

function showQuotationModal() {
    showToast('Abriendo formulario de nueva cotización', 'info');
}

// Update the onSectionChange function to include reports
function onSectionChange(sectionName) {
    // Trigger specific actions when sections change
    switch(sectionName) {
        case 'dashboard':
            updateCharts();
            break;
        case 'calendar':
            renderCalendar();
            break;
        case 'reports':
            renderReportsCharts();
            break;
    }
}

// FullCalendar Implementation
let calendar;
let calendarEvents = [
    {
        id: 'event-1',
        title: 'Reunión con Constructora ABC',
        start: '2024-01-16T10:00:00',
        end: '2024-01-16T11:30:00',
        className: 'event-meeting',
        extendedProps: {
            type: 'meeting',
            client: 'Constructora ABC',
            salesperson: 'Juan Pérez',
            description: 'Presentación de nuevos productos y negociación de precios especiales',
            location: 'Oficina Central',
            status: 'confirmed'
        }
    },
    {
        id: 'event-2',
        title: 'Llamada de seguimiento - Desarrollos XYZ',
        start: '2024-01-17T14:30:00',
        end: '2024-01-17T15:00:00',
        className: 'event-call',
        extendedProps: {
            type: 'call',
            client: 'Desarrollos XYZ',
            salesperson: 'María García',
            description: 'Seguimiento de cotización enviada la semana pasada',
            status: 'pending'
        }
    },
    {
        id: 'event-3',
        title: 'Presentación de productos - Inmobiliaria DEF',
        start: '2024-01-18T11:00:00',
        end: '2024-01-18T12:30:00',
        className: 'event-presentation',
        extendedProps: {
            type: 'presentation',
            client: 'Inmobiliaria DEF',
            salesperson: 'Carlos Ruiz',
            description: 'Demostración de nuevos materiales de construcción',
            location: 'Showroom',
            status: 'confirmed'
        }
    },
    {
        id: 'event-4',
        title: 'Seguimiento post-venta - Grupo Constructor GHI',
        start: '2024-01-19T15:00:00',
        end: '2024-01-19T16:00:00',
        className: 'event-followup',
        extendedProps: {
            type: 'followup',
            client: 'Grupo Constructor GHI',
            salesperson: 'Juan Pérez',
            description: 'Verificar satisfacción con productos entregados',
            status: 'pending'
        }
    },
    {
        id: 'event-5',
        title: 'Vencimiento cotización - Proyectos JKL',
        start: '2024-01-20T23:59:00',
        className: 'event-deadline',
        extendedProps: {
            type: 'deadline',
            client: 'Proyectos JKL',
            salesperson: 'María García',
            description: 'Cotización vence hoy - contactar urgente',
            status: 'urgent'
        }
    },
    {
        id: 'event-6',
        title: 'Reunión de cierre - Edificaciones MNO',
        start: '2024-01-22T09:00:00',
        end: '2024-01-22T10:30:00',
        className: 'event-meeting',
        extendedProps: {
            type: 'meeting',
            client: 'Edificaciones MNO',
            salesperson: 'Carlos Ruiz',
            description: 'Firma de contrato y definición de cronograma de entrega',
            location: 'Oficina del cliente',
            status: 'confirmed'
        }
    },
    {
        id: 'event-7',
        title: 'Llamada técnica - Constructora PQR',
        start: '2024-01-23T16:00:00',
        end: '2024-01-23T16:45:00',
        className: 'event-call',
        extendedProps: {
            type: 'call',
            client: 'Constructora PQR',
            salesperson: 'Juan Pérez',
            description: 'Consulta técnica sobre especificaciones de materiales',
            status: 'pending'
        }
    },
    {
        id: 'event-8',
        title: 'Visita a obra - Desarrollos STU',
        start: '2024-01-24T08:00:00',
        end: '2024-01-24T12:00:00',
        className: 'event-presentation',
        extendedProps: {
            type: 'presentation',
            client: 'Desarrollos STU',
            salesperson: 'María García',
            description: 'Evaluación de necesidades en sitio de construcción',
            location: 'Obra en construcción',
            status: 'confirmed'
        }
    },
    {
        id: 'event-9',
        title: 'Seguimiento semanal - Inmobiliaria VWX',
        start: '2024-01-25T10:30:00',
        end: '2024-01-25T11:00:00',
        className: 'event-followup',
        extendedProps: {
            type: 'followup',
            client: 'Inmobiliaria VWX',
            salesperson: 'Carlos Ruiz',
            description: 'Revisión de avance de proyecto y próximos pedidos',
            status: 'pending'
        }
    },
    {
        id: 'event-10',
        title: 'Vencimiento propuesta - Grupo YZ',
        start: '2024-01-26T23:59:00',
        className: 'event-deadline',
        extendedProps: {
            type: 'deadline',
            client: 'Grupo YZ',
            salesperson: 'Juan Pérez',
            description: 'Propuesta comercial vence - seguimiento necesario',
            status: 'urgent'
        }
    }
];

function initializeFullCalendar() {
    const calendarEl = document.getElementById('fullcalendar');
    if (!calendarEl) return;

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        locale: 'es',
        headerToolbar: false, // We'll use custom toolbar
        height: 'auto',
        events: calendarEvents,
        eventDisplay: 'block',
        dayMaxEvents: 3,
        moreLinkText: 'más',
        
        // Event styling
        eventClassNames: function(arg) {
            return [arg.event.classNames[0]];
        },
        
        // Event click handler
        eventClick: function(info) {
            showEventDetails(info.event);
        },
        
        // Date click handler
        dateClick: function(info) {
            showAddEventModal(info.dateStr);
        },
        
        // View change handler
        datesSet: function(info) {
            updateCalendarTitle(info.view.title);
            updateUpcomingEvents();
            updateEventStatistics();
        },
        
        // Event rendering
        eventDidMount: function(info) {
            // Add tooltip
            info.el.setAttribute('title', 
                `${info.event.title}\n${info.event.extendedProps.client}\nVendedor: ${info.event.extendedProps.salesperson}`
            );
        }
    });

    calendar.render();
    
    // Setup custom toolbar
    setupCalendarToolbar();
    
    // Setup event filters
    setupEventFilters();
    
    // Initial updates
    updateUpcomingEvents();
    updateEventStatistics();
}

function setupCalendarToolbar() {
    // View buttons
    document.querySelectorAll('#calendarViewButtons button').forEach(button => {
        button.addEventListener('click', function() {
            const view = this.getAttribute('data-view');
            calendar.changeView(view);
            
            // Update active button
            document.querySelectorAll('#calendarViewButtons button').forEach(btn => {
                btn.classList.remove('active');
            });
            this.classList.add('active');
        });
    });
    
    // Navigation buttons
    document.getElementById('calendarPrev').addEventListener('click', () => {
        calendar.prev();
    });
    
    document.getElementById('calendarNext').addEventListener('click', () => {
        calendar.next();
    });
    
    document.getElementById('calendarToday').addEventListener('click', () => {
        calendar.today();
    });
}

function setupEventFilters() {
    const filterCheckboxes = [
        'filterMeetings',
        'filterCalls', 
        'filterPresentations',
        'filterFollowups',
        'filterDeadlines'
    ];
    
    filterCheckboxes.forEach(filterId => {
        const checkbox = document.getElementById(filterId);
        if (checkbox) {
            checkbox.addEventListener('change', filterCalendarEvents);
        }
    });
}

function filterCalendarEvents() {
    const filters = {
        meeting: document.getElementById('filterMeetings')?.checked ?? true,
        call: document.getElementById('filterCalls')?.checked ?? true,
        presentation: document.getElementById('filterPresentations')?.checked ?? true,
        followup: document.getElementById('filterFollowups')?.checked ?? true,
        deadline: document.getElementById('filterDeadlines')?.checked ?? true
    };
    
    const filteredEvents = calendarEvents.filter(event => {
        return filters[event.extendedProps.type];
    });
    
    calendar.removeAllEvents();
    calendar.addEventSource(filteredEvents);
}

function updateCalendarTitle(title) {
    const titleElement = document.getElementById('calendarTitle');
    if (titleElement) {
        titleElement.textContent = title;
    }
}

function updateUpcomingEvents() {
    const upcomingList = document.getElementById('upcomingEventsList');
    if (!upcomingList) return;
    
    const now = new Date();
    const nextWeek = new Date(now.getTime() + 7 * 24 * 60 * 60 * 1000);
    
    const upcomingEvents = calendarEvents
        .filter(event => {
            const eventDate = new Date(event.start);
            return eventDate >= now && eventDate <= nextWeek;
        })
        .sort((a, b) => new Date(a.start) - new Date(b.start))
        .slice(0, 5);
    
    upcomingList.innerHTML = '';
    
    if (upcomingEvents.length === 0) {
        upcomingList.innerHTML = '<p class="text-muted text-center">No hay eventos próximos</p>';
        return;
    }
    
    upcomingEvents.forEach(event => {
        const eventDate = new Date(event.start);
        const eventItem = document.createElement('div');
        eventItem.className = `upcoming-event-item event-${event.extendedProps.type}`;
        
        eventItem.innerHTML = `
            <div class="upcoming-event-time">
                ${eventDate.toLocaleDateString('es-ES', { 
                    weekday: 'short', 
                    day: 'numeric', 
                    month: 'short' 
                })} ${eventDate.toLocaleTimeString('es-ES', { 
                    hour: '2-digit', 
                    minute: '2-digit' 
                })}
            </div>
            <div class="upcoming-event-title">${event.title}</div>
            <div class="upcoming-event-client">${event.extendedProps.client}</div>
        `;
        
        eventItem.addEventListener('click', () => {
            const calendarEvent = calendar.getEventById(event.id);
            if (calendarEvent) {
                showEventDetails(calendarEvent);
            }
        });
        
        upcomingList.appendChild(eventItem);
    });
}

function updateEventStatistics() {
    const now = new Date();
    const monthStart = new Date(now.getFullYear(), now.getMonth(), 1);
    const monthEnd = new Date(now.getFullYear(), now.getMonth() + 1, 0);
    
    const monthEvents = calendarEvents.filter(event => {
        const eventDate = new Date(event.start);
        return eventDate >= monthStart && eventDate <= monthEnd;
    });
    
    const stats = {
        meetings: monthEvents.filter(e => e.extendedProps.type === 'meeting').length,
        calls: monthEvents.filter(e => e.extendedProps.type === 'call').length,
        presentations: monthEvents.filter(e => e.extendedProps.type === 'presentation').length,
        followups: monthEvents.filter(e => e.extendedProps.type === 'followup').length,
        deadlines: monthEvents.filter(e => e.extendedProps.type === 'deadline').length,
        completed: monthEvents.filter(e => e.extendedProps.status === 'completed').length,
        pending: monthEvents.filter(e => e.extendedProps.status === 'pending' || e.extendedProps.status === 'confirmed').length
    };
    
    // Update counters
    document.getElementById('meetingsCount').textContent = stats.meetings;
    document.getElementById('callsCount').textContent = stats.calls;
    document.getElementById('presentationsCount').textContent = stats.presentations;
    document.getElementById('followupsCount').textContent = stats.followups;
    document.getElementById('deadlinesCount').textContent = stats.deadlines;
    document.getElementById('completedEventsCount').textContent = stats.completed;
    document.getElementById('pendingEventsCount').textContent = stats.pending;
}

function showEventDetails(event) {
    const props = event.extendedProps;
    const startDate = new Date(event.start);
    const endDate = event.end ? new Date(event.end) : null;
    
    let timeInfo = startDate.toLocaleDateString('es-ES', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    }) + ' a las ' + startDate.toLocaleTimeString('es-ES', {
        hour: '2-digit',
        minute: '2-digit'
    });
    
    if (endDate) {
        timeInfo += ' - ' + endDate.toLocaleTimeString('es-ES', {
            hour: '2-digit',
            minute: '2-digit'
        });
    }
    
    const details = `
Evento: ${event.title}
Cliente: ${props.client}
Vendedor: ${props.salesperson}
Fecha y hora: ${timeInfo}
${props.location ? 'Ubicación: ' + props.location : ''}
${props.description ? 'Descripción: ' + props.description : ''}
Estado: ${getEventStatusText(props.status)}
    `.trim();
    
    alert(details);
}

function getEventStatusText(status) {
    const statusTexts = {
        'pending': 'Pendiente',
        'confirmed': 'Confirmado',
        'completed': 'Completado',
        'cancelled': 'Cancelado',
        'urgent': 'Urgente'
    };
    return statusTexts[status] || status;
}

function showAddEventModal(dateStr = null) {
    showToast('Abriendo formulario de nuevo evento...', 'info');
    // Here you would typically open a modal with a form
    // For demo purposes, we'll just show a toast
}

function syncCalendar() {
    showToast('Sincronizando calendario...', 'info');
    
    // Simulate sync process
    setTimeout(() => {
        updateUpcomingEvents();
        updateEventStatistics();
        showToast('Calendario sincronizado exitosamente', 'success');
    }, 1500);
}

// Update the renderCalendar function to use FullCalendar
function renderCalendar() {
    if (!calendar) {
        initializeFullCalendar();
    } else {
        calendar.render();
        updateUpcomingEvents();
        updateEventStatistics();
    }
}

// Update the onSectionChange function to properly initialize calendar
function onSectionChange(sectionName) {
    // Trigger specific actions when sections change
    switch(sectionName) {
        case 'dashboard':
            updateCharts();
            break;
        case 'calendar':
            // Delay calendar initialization to ensure DOM is ready
            setTimeout(() => {
                renderCalendar();
            }, 100);
            break;
        case 'reports':
            renderReportsCharts();
            break;
    }
}


// Enhanced functions for new features

// Visits management functions
function renderVisitsTable() {
    const tbody = document.getElementById('visitsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    visitsData.forEach(visit => {
        const row = createVisitRow(visit);
        tbody.appendChild(row);
    });
}

function createVisitRow(visit) {
    const tr = document.createElement('tr');
    
    const statusClass = visit.status === 'completada' ? 'bg-success' : 'bg-warning';
    const typeIcon = getVisitTypeIcon(visit.type);
    const documentsCount = visit.documents.length;
    
    tr.innerHTML = `
        <td>
            <div class="fw-bold">${formatDateTime(visit.dateTime)}</div>
        </td>
        <td>${visit.clientName}</td>
        <td>${visit.contactName}</td>
        <td>
            <span class="badge bg-primary">
                <i class="${typeIcon} me-1"></i>${getVisitTypeText(visit.type)}
            </span>
        </td>
        <td>${visit.vendor}</td>
        <td><span class="badge ${statusClass}">${visit.status}</span></td>
        <td>
            <span class="badge bg-info">
                <i class="fas fa-paperclip me-1"></i>${documentsCount}
            </span>
        </td>
        <td>
            <div class="btn-group btn-group-sm">
                <button class="btn btn-outline-primary" onclick="viewVisit(${visit.id})" title="Ver detalles">
                    <i class="fas fa-eye"></i>
                </button>
                <button class="btn btn-outline-success" onclick="editVisit(${visit.id})" title="Editar">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-outline-info" onclick="downloadDocuments(${visit.id})" title="Documentos">
                    <i class="fas fa-download"></i>
                </button>
            </div>
        </td>
    `;
    
    return tr;
}

function getVisitTypeIcon(type) {
    const icons = {
        'presencial': 'fas fa-handshake',
        'virtual': 'fas fa-video',
        'llamada': 'fas fa-phone',
        'email': 'fas fa-envelope',
        'whatsapp': 'fab fa-whatsapp'
    };
    return icons[type] || 'fas fa-comment';
}

function getVisitTypeText(type) {
    const texts = {
        'presencial': 'Presencial',
        'virtual': 'Virtual',
        'llamada': 'Llamada',
        'email': 'Email',
        'whatsapp': 'WhatsApp'
    };
    return texts[type] || type;
}

// Enhanced quotations management
function renderEnhancedQuotationsTable() {
    const tbody = document.getElementById('quotationsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    quotationsData.forEach(quotation => {
        const row = createEnhancedQuotationRow(quotation);
        tbody.appendChild(row);
    });
}

function createEnhancedQuotationRow(quotation) {
    const tr = document.createElement('tr');
    
    const statusClass = getEnhancedQuotationStatusClass(quotation.status);
    const statusText = getEnhancedQuotationStatusText(quotation.status);
    const discountPercent = quotation.discount || 0;
    const finalPriceDisplay = quotation.finalPrice ? `$${quotation.finalPrice.toLocaleString()}` : 'Pendiente';
    
    tr.innerHTML = `
        <td><strong>${quotation.id}</strong></td>
        <td>${quotation.clientName}</td>
        <td>${quotation.vendor}</td>
        <td>${formatDate(quotation.date)}</td>
        <td class="fw-bold text-primary">$${quotation.quotedValue.toLocaleString()}</td>
        <td class="fw-bold text-success">${finalPriceDisplay}</td>
        <td>
            <span class="badge ${discountPercent > 0 ? 'bg-warning' : 'bg-secondary'}">
                ${discountPercent}%
            </span>
        </td>
        <td><span class="badge ${statusClass}">${statusText}</span></td>
        <td>
            <div class="btn-group btn-group-sm">
                <button class="btn btn-outline-primary" onclick="viewQuotationDetails('${quotation.id}')" title="Ver detalles">
                    <i class="fas fa-eye"></i>
                </button>
                <button class="btn btn-outline-success" onclick="editQuotation('${quotation.id}')" title="Editar">
                    <i class="fas fa-edit"></i>
                </button>
                <button class="btn btn-outline-info" onclick="duplicateQuotation('${quotation.id}')" title="Duplicar">
                    <i class="fas fa-copy"></i>
                </button>
                <button class="btn btn-outline-warning" onclick="showPriceHistory('${quotation.id}')" title="Historial">
                    <i class="fas fa-history"></i>
                </button>
            </div>
        </td>
    `;
    
    return tr;
}

function getEnhancedQuotationStatusClass(status) {
    const classes = {
        'cerrada': 'bg-success',
        'en_proceso': 'bg-warning',
        'pendiente': 'bg-info',
        'perdida': 'bg-danger'
    };
    return classes[status] || 'bg-secondary';
}

function getEnhancedQuotationStatusText(status) {
    const texts = {
        'cerrada': 'Cerrada',
        'en_proceso': 'En Proceso',
        'pendiente': 'Pendiente',
        'perdida': 'Perdida'
    };
    return texts[status] || status;
}

// Enhanced leads rendering with new fields
function renderEnhancedLeadsTable() {
    const tbody = document.getElementById('leadsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    leadsData.forEach(lead => {
        const row = createEnhancedLeadRow(lead);
        tbody.appendChild(row);
    });
}

function createEnhancedLeadRow(lead) {
    const tr = document.createElement('tr');
    
    const statusClass = getLeadStatusClass(lead.status);
    const statusText = getLeadStatusText(lead.status);
    const sourceIcon = getLeadSourceIcon(lead.source);
    const typeClass = lead.type === 'client' ? 'bg-success' : 'bg-primary';
    const typeText = lead.type === 'client' ? 'Cliente' : 'Prospecto';
    
    tr.innerHTML = `
        <td>
            <div class="fw-bold">${lead.name}</div>
            <small class="text-muted">${lead.phone}</small>
        </td>
        <td>${lead.company}</td>
        <td>
            <a href="mailto:${lead.email}" class="text-decoration-none">${lead.email}</a>
        </td>
        <td>
            <span class="badge bg-info">
                <i class="${sourceIcon} me-1"></i>${getLeadSourceText(lead.source)}
            </span>
        </td>
        <td>${lead.vendor}</td>
        <td><span class="badge ${typeClass}">${typeText}</span></td>
        <td>
            <div class="d-flex align-items-center">
                <div class="progress flex-grow-1 me-2" style="height: 8px;">
                    <div class="progress-bar ${getScoreColor(lead.score)}" 
                         style="width: ${lead.score}%"></div>
                </div>
                <small class="fw-bold">${lead.score}</small>
            </div>
        </td>
        <td><span class="badge ${statusClass}">${statusText}</span></td>
        <td>
            <div class="btn-group btn-group-sm">
                <button class="btn btn-outline-primary" onclick="viewLead(${lead.id})" title="Ver detalles">
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
    `;
    
    return tr;
}

function getLeadSourceIcon(source) {
    const icons = {
        'website': 'fas fa-globe',
        'expo': 'fas fa-building',
        'campaign': 'fas fa-bullhorn',
        'referral': 'fas fa-share-alt',
        'social': 'fab fa-facebook',
        'email': 'fas fa-envelope',
        'cold-call': 'fas fa-phone'
    };
    return icons[source] || 'fas fa-question';
}

function getLeadSourceText(source) {
    const texts = {
        'website': 'Sitio Web',
        'expo': 'Exposición',
        'campaign': 'Campaña',
        'referral': 'Referencia',
        'social': 'Redes Sociales',
        'email': 'Email',
        'cold-call': 'Llamada Fría'
    };
    return texts[source] || source;
}

// Modal functions for new features
function showAddVisitModal() {
    const modal = new bootstrap.Modal(document.getElementById('addVisitModal'));
    modal.show();
}

function showAddQuotationModal() {
    const modal = new bootstrap.Modal(document.getElementById('addQuotationModal'));
    modal.show();
}

function showAIQuotationAssistant() {
    const modal = new bootstrap.Modal(document.getElementById('aiQuotationModal'));
    modal.show();
}

function saveVisit() {
    const form = document.getElementById('addVisitForm');
    const formData = new FormData(form);
    
    // Here you would normally send data to server
    // For demo purposes, we'll just show a success message
    showToast('Visita guardada exitosamente', 'success');
    
    const modal = bootstrap.Modal.getInstance(document.getElementById('addVisitModal'));
    modal.hide();
    
    // Refresh visits table
    renderVisitsTable();
}

function saveQuotation() {
    const form = document.getElementById('addQuotationForm');
    const formData = new FormData(form);
    
    // Here you would normally send data to server
    // For demo purposes, we'll just show a success message
    showToast('Cotización guardada exitosamente', 'success');
    
    const modal = bootstrap.Modal.getInstance(document.getElementById('addQuotationModal'));
    modal.hide();
    
    // Refresh quotations table
    renderEnhancedQuotationsTable();
}

// AI Assistant functions
function sendAIMessage() {
    const input = document.getElementById('aiChatInput');
    const message = input.value.trim();
    
    if (!message) return;
    
    // Add user message to chat
    addAIMessage('user', message);
    
    // Simulate AI response
    setTimeout(() => {
        const response = generateAIResponse(message);
        addAIMessage('ai', response);
    }, 1000);
    
    input.value = '';
}

function addAIMessage(sender, message) {
    const chatContainer = document.getElementById('aiChatMessages');
    const messageDiv = document.createElement('div');
    messageDiv.className = 'ai-message mb-3';
    
    if (sender === 'user') {
        messageDiv.innerHTML = `
            <div class="d-flex justify-content-end">
                <div class="ai-content bg-primary text-white p-2 rounded">
                    <strong>Tú:</strong> ${message}
                </div>
                <div class="ai-avatar ms-2">
                    <i class="fas fa-user text-primary"></i>
                </div>
            </div>
        `;
    } else {
        messageDiv.innerHTML = `
            <div class="d-flex">
                <div class="ai-avatar me-2">
                    <i class="fas fa-robot text-success"></i>
                </div>
                <div class="ai-content bg-light p-2 rounded">
                    <strong>Asistente IA:</strong> ${message}
                </div>
            </div>
        `;
    }
    
    chatContainer.appendChild(messageDiv);
    chatContainer.scrollTop = chatContainer.scrollHeight;
}

function generateAIResponse(userMessage) {
    const responses = {
        'replicar': 'Puedo ayudarte a replicar la última cotización. ¿Para qué cliente la necesitas?',
        'productos': 'Basándome en el historial, te sugiero incluir: Cemento Portland, Varilla de Acero #4, y Ladrillo Rojo.',
        'descuentos': 'Para este cliente aplica la política de descuento del 15-20%. ¿Quieres que lo configure automáticamente?',
        'historico': 'El precio histórico promedio para este producto es $285.50. La última venta fue a $256.95 con 10% de descuento.'
    };
    
    for (let key in responses) {
        if (userMessage.toLowerCase().includes(key)) {
            return responses[key];
        }
    }
    
    return 'Entiendo tu consulta. ¿Podrías ser más específico sobre qué tipo de cotización necesitas crear?';
}

function aiQuickAction(action) {
    const responses = {
        'replicar': 'He encontrado tu última cotización para Constructora ABC por $187,500. ¿Quieres usar esta como base?',
        'productos': 'Sugiero estos productos populares: Cemento Portland (100 unidades), Varilla de Acero #4 (500 unidades), Ladrillo Rojo (10,000 unidades).',
        'descuentos': 'Para Constructora ABC aplica descuento del 15-20%. Para Desarrollos XYZ: 10-12%. ¿Cuál cliente necesitas?',
        'historico': 'Precios históricos: Cemento $285.50 (actual), Varilla $12.75 (actual), Ladrillo $3.25 (actual). Todos estables últimos 30 días.'
    };
    
    addAIMessage('ai', responses[action]);
}

function applyAISuggestions() {
    showToast('Sugerencias de IA aplicadas a la cotización', 'success');
    const modal = bootstrap.Modal.getInstance(document.getElementById('aiQuotationModal'));
    modal.hide();
}

// Discount analysis chart
function initializeDiscountAnalysisChart() {
    const ctx = document.getElementById('discountAnalysisChart');
    if (!ctx) return;
    
    if (charts.discountAnalysis) {
        charts.discountAnalysis.destroy();
    }
    
    charts.discountAnalysis = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['0-5%', '6-10%', '11-15%', '16-20%', '20%+'],
            datasets: [{
                data: [15, 35, 25, 20, 5],
                backgroundColor: [
                    '#e3f2fd',
                    '#bbdefb',
                    '#90caf9',
                    '#64b5f6',
                    '#42a5f5'
                ],
                borderColor: [
                    '#1976d2',
                    '#1976d2',
                    '#1976d2',
                    '#1976d2',
                    '#1976d2'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 10,
                        usePointStyle: true
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.label + ': ' + context.parsed + '%';
                        }
                    }
                }
            }
        }
    });
}

// Utility functions
function formatDateTime(dateTimeString) {
    const date = new Date(dateTimeString);
    return date.toLocaleDateString('es-ES') + ' ' + date.toLocaleTimeString('es-ES', {hour: '2-digit', minute: '2-digit'});
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES');
}

function getScoreColor(score) {
    if (score >= 80) return 'bg-success';
    if (score >= 60) return 'bg-info';
    if (score >= 40) return 'bg-warning';
    return 'bg-danger';
}

// Enhanced navigation function to handle new sections
function showSection(sectionName) {
    // Hide all sections
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });
    
    // Show selected section
    const section = document.getElementById(sectionName);
    if (section) {
        section.style.display = 'block';
        
        // Update active nav link
        document.querySelectorAll('.nav-link').forEach(link => {
            link.classList.remove('active');
        });
        document.querySelector(`[data-section="${sectionName}"]`).classList.add('active');
        
        // Initialize section-specific content
        switch(sectionName) {
            case 'visits':
                renderVisitsTable();
                break;
            case 'quotations':
                renderEnhancedQuotationsTable();
                initializeDiscountAnalysisChart();
                break;
            case 'leads':
                renderEnhancedLeadsTable();
                break;
            default:
                // Call original initialization if exists
                if (typeof initializeSection === 'function') {
                    initializeSection(sectionName);
                }
        }
    }
}

// Enhanced initialization
document.addEventListener('DOMContentLoaded', function() {
    // Initialize enhanced features
    renderEnhancedLeadsTable();
    renderVisitsTable();
    renderEnhancedQuotationsTable();
    
    // Initialize enhanced navigation
    document.querySelectorAll('[data-section]').forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const sectionName = this.getAttribute('data-section');
            showSection(sectionName);
        });
    });
    
    // Initialize AI chat input
    const aiChatInput = document.getElementById('aiChatInput');
    if (aiChatInput) {
        aiChatInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                sendAIMessage();
            }
        });
    }
});
