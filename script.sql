-- Ejecutar completo en SQL Server Management Studio (botón Execute / F5)
CREATE DATABASE ProductosDB;
GO
USE ProductosDB;
GO
CREATE TABLE Productos (
    Id     INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock  INT NOT NULL DEFAULT 0
);
GO
INSERT INTO Productos (Nombre, Precio, Stock) VALUES
('Laptop', 15000.00, 5),
('Mouse', 250.00, 40),
('Teclado', 600.00, 20);
GO
