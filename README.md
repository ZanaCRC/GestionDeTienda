# Sistema de Gestión de Tienda

## Descripción
Sistema completo para la gestión de tiendas que permite administrar inventarios, ventas, usuarios, cajas y ajustes de inventario. Está desarrollado con tecnología .NET y sigue una arquitectura en capas para facilitar el mantenimiento y la escalabilidad.

## Características Principales
- Gestión de inventario (agregar, modificar, eliminar productos)
- Sistema de ventas con carrito de compras
- Administración de usuarios con diferentes roles
- Control de cajas (apertura y cierre de cajas)
- Ajustes de inventario para correcciones y actualizaciones
- Autenticación mediante Google y Facebook
- Generación de reportes e históricos
- Interfaz web y aplicación móvil

## Estructura del Proyecto
El proyecto está organizado en las siguientes capas:

### GestionDeTiendaParte2.UI
Interfaz de usuario web desarrollada con ASP.NET MVC. Contiene las vistas, controladores y modelos de la aplicación web.

### GestionDeTiendaParte2.App
Aplicación móvil desarrollada con .NET MAUI para proporcionar acceso al sistema desde dispositivos móviles.

### GestionDeTiendaParte2.BL (Business Logic)
Contiene la lógica de negocio del sistema, implementando los servicios para:
- AdministradorDeUsuarios
- AdministradorDeVentas
- AdministradorDeCajas
- AdministradorDeInventarios
- AdministradorDeAjustesDeInventarios
- AdministradorDeCorreos

### GestionDeTiendaParte2.Model
Define las entidades y modelos de datos utilizados en toda la aplicación:
- Usuario y roles
- Inventario y categorías
- Ventas y detalles
- Cajas y aperturas
- Ajustes de inventario

### GestionDeTiendaParte2.DA (Data Access)
Capa de acceso a datos utilizando Entity Framework Core para la comunicación con la base de datos SQL Server.

### GestionDeTiendaParte2.SI (Service Interface)
Interfaces de servicios para la integración con sistemas externos.

## Requisitos
- .NET 6.0 o superior
- SQL Server
- Visual Studio 2022
- Credenciales para autenticación con Google y Facebook (opcional)

## Instalación
1. Clone el repositorio:
```
git clone https://github.com/tuusuario/GestionDeTienda.git
```

2. Abra la solución `GestionDeTiendaParte2.sln` en Visual Studio.

3. Configure la cadena de conexión en `appsettings.json` para apuntar a su instancia de SQL Server.

4. Ejecute las migraciones para crear la base de datos:
```
Update-Database
```

5. Configure los secretos para autenticación con proveedores externos (opcional):
```
dotnet user-secrets set "GoogleKeys:ClientId" "su-client-id-de-google"
dotnet user-secrets set "GoogleKeys:ClientSecret" "su-client-secret-de-google"
dotnet user-secrets set "Facebook_Keys:ClientId" "su-client-id-de-facebook"
dotnet user-secrets set "Facebook_Keys:ClientSecret" "su-client-secret-de-facebook"
```

6. Ejecute la aplicación.

## Uso
1. Acceda a la aplicación web a través de la URL local (por defecto: https://localhost:5001).
2. Inicie sesión utilizando las credenciales predeterminadas o configure nuevos usuarios.
3. Utilice el panel de navegación para acceder a las diferentes funcionalidades del sistema.

## Funcionalidades Principales

### Gestión de Usuarios
- Registro e inicio de sesión de usuarios
- Administración de roles y permisos
- Autenticación por redes sociales

### Inventario
- Agregar y modificar productos
- Gestionar categorías
- Realizar ajustes de inventario
- Consultar histórico de cambios

### Ventas
- Crear y procesar ventas
- Aplicar descuentos
- Seleccionar método de pago
- Generar recibos

### Cajas
- Apertura y cierre de cajas
- Control de efectivo
- Reportes de transacciones

## Contribución
1. Haga un fork del repositorio
2. Cree una rama para su característica (`git checkout -b feature/nueva-caracteristica`)
3. Commit sus cambios (`git commit -m 'Añadir nueva característica'`)
4. Push a la rama (`git push origin feature/nueva-caracteristica`)
5. Abra un Pull Request
