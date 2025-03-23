# 🍕 ItaliaPizza - Sistema de Gestión para Restaurante

**ItaliaPizza** es un proyecto escolar desarrollado para la EE: **Desarrollo de Software**.  
Este sistema está compuesto por los siguientes módulos:

- Módulo Usuarios
- Módulo Productos
- Módulo Pedidos
- Módulo de Cocina
- Módulo de Finanzas

---

## Clonar el repositorio 💻

Para obtener el código fuente, ejecuta el siguiente comando en la carpeta destino.

```bash
git clone https://github.com/TonyVHT/ItaliaPizza.git
```

## Hacer migraciones en la base de datos (Code first) 💻
Para hacer las migraciones ejecutar los siguientes comandos

```
## Crear la migración
dotnet ef migrations add NombreDeLaMigracion --project ItaliaPizza.Server

## Actualizar la db con la nueva migración
dotnet ef database update NombreDeLaMigracionAnterior --project ItaliaPizza.Server

```

## 🗃️ Uso de la base de datos

Para crear la base de datos utilizando la metodología **Code First**, se deben seguir los siguientes pasos:

- 1) Crear un usuario en Sql Server. Asegúrate de darle permisos para crear y adminitrar la base de datos.
- 2) Defnir las siguientes variables del sistema.
```
ITALIAPIZZA_DB_SERVER = Nombre de tu servidor en SqlServer
ITALIAPIZZA_DB_NAME = Nombre de la base de datos
ITALIAPIZZA_DB_USER = Usuario de la base de datos
ITALIAPIZZA_DB_PASSWORD = Contraseña del usuario
```

> 📝 **Nota:** Asegúrate de que el usuario creado en SQL Server coincida con los datos definidos en las variables de entorno.

### 2. Definir las variables de entorno del sistema
Debes guardar las siguientes variables de entorno en tu sistema operativo. Estas serán utilizadas por la aplicación para construir la cadena de conexión:


## Requisitos para el desarrollo
- Sistema operativo Windows 2010 o 2011 (Para WPF)
- .Net core SDK 9
- Editor de texto
- Sql server
- Para prueba: Curl o Postman
- De preferencia contar con Visual Studio Community 2022

## Integrantes del equipo 🤓
Aguilar Aguilar Marla Jasel  
Martínez Ramírez Fernando  
Villegas Hurtado Tony  
Torres Ortiz Juan Pablo  
