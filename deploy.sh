#!/bin/bash

# CRM System Deployment Script
# This script sets up and deploys the complete CRM system

echo "🚀 CRM System Deployment Script"
echo "================================"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check prerequisites
check_prerequisites() {
    print_status "Checking prerequisites..."
    
    # Check .NET SDK
    if command -v dotnet &> /dev/null; then
        DOTNET_VERSION=$(dotnet --version)
        print_success ".NET SDK found: $DOTNET_VERSION"
    else
        print_error ".NET SDK not found. Please install .NET 8.0 SDK"
        exit 1
    fi
    
    # Check SQL Server connection (optional)
    print_warning "Make sure SQL Server is accessible with the connection string in appsettings.json"
}

# Setup database
setup_database() {
    print_status "Setting up database..."
    
    if [ -f "crm-sqlserver-schema.sql" ]; then
        print_success "Database schema file found"
        print_warning "Please execute crm-sqlserver-schema.sql in your SQL Server Management Studio"
        print_warning "Or use sqlcmd: sqlcmd -S (localdb)\\mssqllocaldb -i crm-sqlserver-schema.sql"
    else
        print_error "Database schema file not found"
    fi
}

# Build application
build_application() {
    print_status "Building CRM application..."
    
    cd CRMSystem.API
    
    # Restore packages
    print_status "Restoring NuGet packages..."
    dotnet restore
    
    if [ $? -eq 0 ]; then
        print_success "Packages restored successfully"
    else
        print_error "Failed to restore packages"
        exit 1
    fi
    
    # Build application
    print_status "Building application..."
    dotnet build --configuration Release
    
    if [ $? -eq 0 ]; then
        print_success "Application built successfully"
    else
        print_error "Failed to build application"
        exit 1
    fi
    
    cd ..
}

# Run application
run_application() {
    print_status "Starting CRM application..."
    
    cd CRMSystem.API
    
    print_success "🎉 CRM System is starting..."
    print_success "📊 Dashboard: http://localhost:5000"
    print_success "📚 API Documentation: http://localhost:5000/swagger"
    print_success "🔍 Health Check: http://localhost:5000/health"
    print_warning "Press Ctrl+C to stop the application"
    
    # Run the application
    dotnet run --urls="http://localhost:5000"
}

# Publish application for deployment
publish_application() {
    print_status "Publishing application for deployment..."
    
    cd CRMSystem.API
    
    # Create publish directory
    mkdir -p ../publish
    
    # Publish application
    dotnet publish --configuration Release --output ../publish
    
    if [ $? -eq 0 ]; then
        print_success "Application published to ./publish directory"
        print_status "Deployment files:"
        ls -la ../publish/
    else
        print_error "Failed to publish application"
        exit 1
    fi
    
    cd ..
}

# Create Docker image (optional)
create_docker_image() {
    print_status "Creating Docker image..."
    
    if command -v docker &> /dev/null; then
        # Create Dockerfile if it doesn't exist
        if [ ! -f "Dockerfile" ]; then
            cat > Dockerfile << EOF
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CRMSystem.API/CRMSystem.API.csproj", "CRMSystem.API/"]
RUN dotnet restore "CRMSystem.API/CRMSystem.API.csproj"
COPY . .
WORKDIR "/src/CRMSystem.API"
RUN dotnet build "CRMSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CRMSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CRMSystem.API.dll"]
EOF
        fi
        
        # Build Docker image
        docker build -t crm-system:latest .
        
        if [ $? -eq 0 ]; then
            print_success "Docker image created: crm-system:latest"
            print_status "Run with: docker run -p 5000:80 crm-system:latest"
        else
            print_error "Failed to create Docker image"
        fi
    else
        print_warning "Docker not found. Skipping Docker image creation."
    fi
}

# Main deployment menu
show_menu() {
    echo ""
    echo "🎯 CRM System Deployment Options:"
    echo "1. Check Prerequisites"
    echo "2. Setup Database"
    echo "3. Build Application"
    echo "4. Run Application (Development)"
    echo "5. Publish Application (Production)"
    echo "6. Create Docker Image"
    echo "7. Full Deployment (1-4)"
    echo "8. Exit"
    echo ""
}

# Main script execution
main() {
    echo "Welcome to CRM System Deployment!"
    echo ""
    
    while true; do
        show_menu
        read -p "Select an option (1-8): " choice
        
        case $choice in
            1)
                check_prerequisites
                ;;
            2)
                setup_database
                ;;
            3)
                build_application
                ;;
            4)
                run_application
                ;;
            5)
                publish_application
                ;;
            6)
                create_docker_image
                ;;
            7)
                print_status "Starting full deployment..."
                check_prerequisites
                setup_database
                build_application
                run_application
                ;;
            8)
                print_success "Goodbye! 👋"
                exit 0
                ;;
            *)
                print_error "Invalid option. Please select 1-8."
                ;;
        esac
        
        echo ""
        read -p "Press Enter to continue..."
    done
}

# Run main function
main
