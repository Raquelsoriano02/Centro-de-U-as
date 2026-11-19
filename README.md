# Centro de Uñas: Sistema de Turnos

Sistema para gestionar los turnos de un centro de uñas. Permite registrar clientes, técnicas, empleados y servicios, definir el horario de trabajo de cada técnica, agendar turnos solo en horas libres y registrar los pagos de cada turno.

Proyecto final de Programación 2, hecho por Raquel Andrea Soriano Cespedes.

## Idea del proyecto

En un centro de uñas los turnos suelen anotarse en una libreta o por mensajes. Eso provoca dos clientes a la misma hora con la misma técnica, turnos fuera del horario de trabajo y pagos que no se sabe a qué servicio corresponden.

Este sistema resuelve eso:

- **Agenda sin choques:** solo se puede agendar en una hora en la que la técnica trabaja y no tiene otro turno.
- **Búsqueda de horarios libres:** para un día o para un rango de hasta 31 días, según la duración del servicio.
- **Seguimiento del turno:** cada turno pasa por los estados Pendiente, Confirmado, Completado o Cancelado.
- **Control de pagos:** un turno puede pagarse en partes, pero nunca por encima de su precio.
- **Registro de quién atendió:** cada turno guarda el empleado que lo registró.

### Objetivos

1. Evitar turnos duplicados o fuera del horario de la técnica.
2. Tener en un solo lugar los clientes, servicios, turnos y pagos.
3. Aplicar los conceptos de Programación Orientada a Objetos en un sistema real con base de datos, capas separadas y un frontend independiente.

## Tecnologías

| Parte | Tecnología |
|---|---|
| Backend | ASP.NET Core 9 Web API con Swagger |
| Base de datos | SQL Server con Entity Framework Core 9 (migraciones) |
| Frontend | HTML, CSS y JavaScript puro (módulos ES), servido por un proyecto ASP.NET Core |
| Lenguaje | C# |

## Arquitectura

La solución tiene cinco proyectos. El frontend y la API son aplicaciones separadas que se comunican por HTTP (arquitectura distribuida cliente-servidor), y la API está dividida en capas.

```mermaid
flowchart LR
    subgraph Cliente["Navegador"]
        Web["CentroUnas.Frontend<br/>HTML, CSS y JavaScript"]
    end
    subgraph Servidor["API REST"]
        Api["Centro de uñas<br/>Controladores"]
        App["CentroUnas.Application<br/>Servicios y DTO"]
        Infra["CentroUnas.Infrastructure<br/>Repositorios y DbContext"]
        Dom["CentroUnas.Domain<br/>Entidades"]
    end
    DB[("SQL Server")]

    Web -- "HTTP / JSON" --> Api
    Api --> App
    App --> Infra
    Infra --> Dom
    App --> Dom
    Infra --> DB
```

| Capa | Proyecto | Responsabilidad |
|---|---|---|
| Presentación | `CentroUnas.Frontend` | Página web en JavaScript puro. Llama a la API con `fetch` desde `js/api.js`. |
| API | `Centro de uñas` | Controladores REST, CORS y Swagger. |
| Aplicación | `CentroUnas.Application` | Servicios que convierten entidades en DTO y devuelven un `ServiceResult`. |
| Infraestructura | `CentroUnas.Infrastructure` | `DbContext`, migraciones, repositorios y reglas de negocio. |
| Dominio | `CentroUnas.Domain` | Entidades, clases abstractas y enumeraciones. |

## Diagrama de clases

### Dominio

```mermaid
classDiagram
    direction TB

    class BaseEntity {
        <<abstract>>
        +int Id
        +DateTime FechaRegistro
        +ObtenerDescripcion()* string
    }

    class Person {
        <<abstract>>
        +string Nombre
        +string Apellido
        +string Email
        +string Telefono
        #Person()
        #Person(nombre, apellido, telefono)
        #Person(nombre, apellido, telefono, email)
        +ObtenerNombreCompleto() string
        +ObtenerRol()* string
        +ObtenerDescripcion() string
    }

    class Cliente {
        +Cliente()
        +Cliente(nombre, apellido, telefono)
        +Cliente(nombre, apellido, telefono, email)
        +ObtenerRol() string
    }

    class Empleado {
        +string Cargo
        +bool Activo
        +Empleado()
        +Empleado(nombre, apellido, telefono, cargo)
        +Empleado(nombre, apellido, telefono, email, cargo)
        +ObtenerRol() string
    }

    class Tecnica {
        +string Especialidad
        +bool Activa
        +Tecnica()
        +Tecnica(nombre, apellido, telefono, especialidad)
        +Tecnica(nombre, apellido, telefono, email, especialidad)
        +ObtenerRol() string
    }

    class Servicio {
        +string Nombre
        +string Descripcion
        +decimal Precio
        +int DuracionMinutos
        +bool Activo
        +Servicio()
        +Servicio(nombre, precio, duracionMinutos)
        +Servicio(nombre, precio, duracionMinutos, descripcion)
        +ObtenerDescripcion() string
    }

    class Horario {
        +int TecnicaId
        +DiaSemana DiaSemana
        +TimeOnly HoraInicio
        +TimeOnly HoraFin
        +bool Disponible
        +Horario()
        +Horario(tecnicaId, diaSemana, horaInicio, horaFin)
        +Horario(tecnicaId, diaSemana, horaInicio, horaFin, disponible)
        +ObtenerDescripcion() string
    }

    class Turno {
        +int ClienteId
        +int EmpleadoId
        +int TecnicaId
        +int ServicioId
        +DateTime FechaHoraInicio
        +DateTime FechaHoraFin
        +decimal Precio
        +EstadoTurno Estado
        +string Observacion
        +Turno()
        +Turno(clienteId, empleadoId, tecnicaId, servicioId, fechaHoraInicio)
        +Turno(clienteId, empleadoId, tecnicaId, servicioId, fechaHoraInicio, observacion)
        +ObtenerDescripcion() string
    }

    class Pago {
        +int TurnoId
        +decimal Monto
        +MetodoPago MetodoPago
        +DateTime FechaPago
        +string Referencia
        +Pago()
        +Pago(turnoId, monto, metodoPago)
        +Pago(turnoId, monto, metodoPago, referencia)
        +ObtenerDescripcion() string
    }

    class EstadoTurno {
        <<enumeration>>
        Pendiente
        Confirmado
        Completado
        Cancelado
    }

    class MetodoPago {
        <<enumeration>>
        Efectivo
        Tarjeta
        Transferencia
    }

    class DiaSemana {
        <<enumeration>>
        Domingo
        Lunes
        Martes
        Miercoles
        Jueves
        Viernes
        Sabado
    }

    BaseEntity <|-- Person
    BaseEntity <|-- Servicio
    BaseEntity <|-- Horario
    BaseEntity <|-- Turno
    BaseEntity <|-- Pago
    Person <|-- Cliente
    Person <|-- Empleado
    Person <|-- Tecnica

    Cliente "1" --> "0..*" Turno : solicita
    Empleado "1" --> "0..*" Turno : registra
    Tecnica "1" --> "0..*" Turno : atiende
    Servicio "1" --> "0..*" Turno : se realiza en
    Tecnica "1" *-- "0..*" Horario : trabaja en
    Turno "1" --> "0..*" Pago : se paga con

    Turno ..> EstadoTurno
    Pago ..> MetodoPago
    Horario ..> DiaSemana
```

Los métodos marcados con `*` son abstractos. `Email`, `Descripcion`, `Observacion` y `Referencia` son opcionales.

### Capas de la API

```mermaid
classDiagram
    direction LR

    class BaseRepository~T~ {
        #CentroUnasContext _context
        +GetById(id) T
        +GetAll() List~T~
        +Create(entity) T
        +Update(entity) T
        +Delete(id) T
    }

    class TurnoRepository {
        +GetTurnosAsync(fecha) List~Turno~
        +GetTurnosAsync(fecha, tecnicaId) List~Turno~
        +GetDisponibilidadAsync(tecnicaId, servicioId, fecha) List~DateTime~
        +GetDisponibilidadAsync(tecnicaId, servicioId, desde, hasta) List~DateTime~
        +CreateTurnoAsync(datos) Turno
        +UpdateTurnoAsync(id, datos) bool
        +UpdateEstadoAsync(id, estado) bool
        +DeleteTurnoAsync(id) bool
    }

    class ClienteRepository
    class EmpleadoRepository
    class TecnicaRepository
    class ServicioRepository
    class HorarioRepository
    class PagoRepository

    class TurnoService {
        -TurnoRepository _repository
        +GetTurnosAsync(fecha) ServiceResult
        +GetTurnosAsync(fecha, tecnicaId) ServiceResult
        +GetDisponibilidadAsync(tecnicaId, servicioId, fecha) ServiceResult
        +GetDisponibilidadAsync(tecnicaId, servicioId, desde, hasta) ServiceResult
        +CreateTurnoAsync(dto) ServiceResult
    }

    class ServiceResult~T~ {
        +bool Success
        +bool NotFound
        +string Message
        +T Data
        +Ok(data)$ ServiceResult~T~
        +Fail(message)$ ServiceResult~T~
        +NotFoundResult(message)$ ServiceResult~T~
    }

    class DtoBase {
        <<abstract>>
        +int Id
        +DateTime FechaRegistro
        +string Resumen
    }

    class TurnoController {
        -TurnoService _service
    }

    BaseRepository~T~ <|-- TurnoRepository
    BaseRepository~T~ <|-- ClienteRepository
    BaseRepository~T~ <|-- EmpleadoRepository
    BaseRepository~T~ <|-- TecnicaRepository
    BaseRepository~T~ <|-- ServicioRepository
    BaseRepository~T~ <|-- HorarioRepository
    BaseRepository~T~ <|-- PagoRepository

    TurnoController --> TurnoService
    TurnoService --> TurnoRepository
    TurnoService ..> ServiceResult~T~
    TurnoService ..> DtoBase
```

Los otros recursos (Cliente, Empleado, Técnica, Servicio, Horario y Pago) siguen el mismo esquema que Turno: un controlador, un servicio y un repositorio.

## Conceptos de POO aplicados

| Concepto | Dónde se aplica |
|---|---|
| **Clases normales** | Las entidades `Cliente`, `Empleado`, `Tecnica`, `Servicio`, `Horario`, `Turno` y `Pago` (`CentroUnas.Domain/Entities`), además de repositorios, servicios y controladores. |
| **Constructores** | Cada entidad tiene un constructor vacío (lo usa Entity Framework) y constructores con parámetros. Los servicios y repositorios crean los objetos con ellos, por ejemplo `new Cliente(nombre, apellido, telefono, email)`. |
| **Encadenamiento de constructores** | Las clases hijas llaman al padre con `: base(...)` (`Cliente` → `Person`) y las versiones largas reutilizan la corta con `: this(...)`. |
| **Clases abstractas** | `BaseEntity` y `Person` en el dominio, y `DtoBase` en Application. |
| **Métodos abstractos** | `BaseEntity.ObtenerDescripcion()` lo implementan todas las entidades. `Person.ObtenerRol()` lo implementan `Cliente`, `Empleado` y `Tecnica`. El resultado llega al usuario en el campo `Resumen`. |
| **Sobrecarga de métodos** | `TurnoRepository` y `TurnoService`: `GetDisponibilidadAsync` (un día o un rango) y `GetTurnosAsync` (por fecha o por fecha y técnica). El frontend las usa al buscar horas libres de un día o de 7 días y al filtrar la agenda de hoy por técnica. Los constructores también están sobrecargados. |
| **Herencia** | `Cliente`, `Empleado` y `Tecnica` heredan de `Person`, que hereda de `BaseEntity`. Los repositorios heredan de `BaseRepository<T>`. |
| **Polimorfismo** | `ObtenerDescripcion()` se llama igual en cualquier entidad y cada una responde con su propio texto. |
| **Encapsulamiento** | `ServiceResult<T>` tiene setters privados y solo se crean con `Ok`, `Fail` o `NotFoundResult`. Los repositorios exponen `_context` solo a sus clases hijas (`protected`). |
| **Genéricos** | `BaseRepository<T>` y `ServiceResult<T>`. |

## Reglas de negocio

- Un turno solo se agenda en el futuro, dentro de un horario disponible de la técnica.
- La técnica no puede tener dos turnos que se crucen, y el cliente tampoco.
- La técnica, el empleado y el servicio del turno deben estar activos.
- La hora de fin se calcula con la duración del servicio y el precio se copia del servicio.
- Un turno completado o cancelado ya no se puede modificar, y un turno con pagos no se puede cancelar ni eliminar.
- Un pago no puede superar el saldo pendiente del turno ni registrarse en un turno cancelado.
- Los horarios de una técnica no pueden cruzarse, y la hora de inicio debe ser menor que la de fin.
- El correo de clientes, técnicas y empleados no se puede repetir.
- No se puede eliminar un cliente, técnica, empleado o servicio que tenga turnos.

## Endpoints principales

Todos los recursos tienen `GET`, `GET /{id}`, `POST`, `PUT /{id}` y `DELETE /{id}` en `api/Cliente`, `api/Empleado`, `api/Tecnica`, `api/Servicio`, `api/Horario`, `api/Turno` y `api/Pago`. Además, Turno tiene:

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `api/Turno/fecha/{fecha}?tecnicaId=` | Turnos de un día, opcionalmente de una técnica |
| `GET` | `api/Turno/disponibilidad?tecnicaId=&servicioId=&fecha=` | Horas libres de un día |
| `GET` | `api/Turno/disponibilidad/rango?tecnicaId=&servicioId=&desde=&hasta=` | Horas libres en un rango de hasta 31 días |
| `PUT` | `api/Turno/{id}/estado` | Cambia el estado del turno |

La documentación completa está en Swagger: `http://localhost:5228/swagger`.

## Cómo ejecutarlo

### Requisitos

- .NET SDK 9
- SQL Server (la cadena de conexión usa `Server=(local)` con autenticación de Windows)
- Visual Studio 2022 (opcional)

### Pasos

1. Clonar el repositorio.
2. Revisar la cadena de conexión en `Centro de uñas/Centro de uñas/appsettings.Development.json`.
3. Crear la base de datos con las migraciones:

   ```
   cd "Centro de uñas"
   dotnet ef database update --project CentroUnas.Infrastructure --startup-project "Centro de uñas"
   ```

4. Iniciar la API y el frontend:
   - **Visual Studio:** abrir `Centro de uñas.sln`, elegir el perfil de inicio **API + Frontend** y presionar F5. Se abren Swagger y la página web.
   - **Terminal:** en dos terminales distintas, desde la carpeta `Centro de uñas`:

     ```
     dotnet run --project "Centro de uñas" --launch-profile http
     dotnet run --project CentroUnas.Frontend --launch-profile http
     ```

5. Abrir el frontend en `http://localhost:5229`. La API queda en `http://localhost:5228`.

El frontend busca la API en la dirección de `CentroUnas.Frontend/wwwroot/js/config.js`, y la API solo acepta llamadas de las direcciones de `Cors:Origenes` en su `appsettings.json`. Si se cambia algún puerto, hay que actualizar los dos archivos.

## Estructura del repositorio

```
Centro de uñas/
├── Centro de uñas/              API: controladores, Program.cs, appsettings
├── CentroUnas.Application/      Servicios, DTO y ServiceResult
├── CentroUnas.Domain/           Entidades, clases abstractas y enumeraciones
├── CentroUnas.Infrastructure/   DbContext, migraciones, repositorios y excepciones
└── CentroUnas.Frontend/         Frontend en JavaScript puro
    ├── Program.cs               Sirve los archivos de wwwroot
    └── wwwroot/
        ├── index.html           Página única con menú lateral
        ├── css/styles.css       Estilos con tema claro y oscuro
        └── js/
            ├── config.js        Dirección de la API
            ├── api.js           Llamadas HTTP a la API
            ├── app.js           Menú, tema y rutas
            ├── modules.js       Columnas, campos y acciones de cada pantalla
            ├── crud.js          Tabla con búsqueda, filtros, orden y paginación
            ├── form.js          Formularios y selector de horas libres
            ├── dashboard.js     Página de inicio
            ├── expediente.js    Historial de cada cliente
            └── ui.js            Elementos, iconos, formatos, avisos y modales
```
