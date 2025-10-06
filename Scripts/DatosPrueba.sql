-- Script de Datos de Prueba para AutoMax Concesionario
-- Ejecutar despu�s de crear la base de datos

USE concesionario;
GO

-- Insertar Tipos de Veh�culos
INSERT INTO tipo_vehiculo (nombre) VALUES 
('Sed�n'),
('SUV'),
('Deportivo'),
('Camioneta'),
('Hatchback'),
('Convertible'),
('Station Wagon'),
('Coup�');

-- Insertar Tipos de Conductores
INSERT INTO tipo_conductor (nombre) VALUES 
('Particular'),
('Comercial'),
('Profesional'),
('Servicio P�blico'),
('Motocicleta'),
('Transporte Escolar');

-- Insertar Veh�culos de Prueba
INSERT INTO vehiculo (marca, modelo, matricula, a�o, id_tipo_vehiculo) VALUES 
('Toyota', 'Corolla', 'ABC-123', 2022, 1),
('Honda', 'CR-V', 'XYZ-789', 2023, 2),
('Nissan', 'Sentra', 'DEF-456', 2021, 1),
('Ford', 'Explorer', 'GHI-789', 2022, 2),
('Chevrolet', 'Camaro', 'JKL-012', 2023, 3),
('BMW', 'X5', 'MNO-345', 2022, 2),
('Mercedes-Benz', 'C-Class', 'PQR-678', 2021, 1),
('Audi', 'A4', 'STU-901', 2023, 1),
('Volkswagen', 'Golf', 'VWX-234', 2022, 5),
('Hyundai', 'Tucson', 'YZA-567', 2023, 2);

-- Insertar Conductores de Prueba
INSERT INTO conductor (cedula, nombre, apellido, fecha_nacimiento, telefono, direccion, id_tipo_conductor) VALUES 
('12345678', 'Juan', 'P�rez', '1985-05-15', '555-0101', 'Calle 123 #45-67', 1),
('87654321', 'Mar�a', 'Gonz�lez', '1990-08-22', '555-0102', 'Carrera 89 #12-34', 2),
('11111111', 'Carlos', 'Rodr�guez', '1982-12-03', '555-0103', 'Avenida 56 #78-90', 3),
('22222222', 'Ana', 'L�pez', '1995-03-18', '555-0104', 'Diagonal 12 #34-56', 1),
('33333333', 'Pedro', 'Mart�nez', '1988-07-25', '555-0105', 'Transversal 78 #90-12', 2);

-- Verificar datos insertados
SELECT 'Tipos de Veh�culos' as Tabla, COUNT(*) as Registros FROM tipo_vehiculo
UNION ALL
SELECT 'Tipos de Conductores', COUNT(*) FROM tipo_conductor
UNION ALL
SELECT 'Veh�culos', COUNT(*) FROM vehiculo
UNION ALL
SELECT 'Conductores', COUNT(*) FROM conductor;

-- Consulta para ver veh�culos con su tipo
SELECT 
    v.marca,
    v.modelo,
    v.matricula,
    v.a�o,
    tv.nombre as TipoVehiculo
FROM vehiculo v
INNER JOIN tipo_vehiculo tv ON v.id_tipo_vehiculo = tv.id
ORDER BY v.marca, v.modelo;

-- Consulta para ver conductores con su tipo de licencia
SELECT 
    c.nombre + ' ' + c.apellido as NombreCompleto,
    c.cedula,
    c.telefono,
    tc.nombre as TipoLicencia
FROM conductor c
INNER JOIN tipo_conductor tc ON c.id_tipo_conductor = tc.id
ORDER BY c.apellido, c.nombre;