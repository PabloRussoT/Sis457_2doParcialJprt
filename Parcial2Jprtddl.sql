CREATE DATABASE Parcial2Jprt;
GO
USE [master]
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD = N'12345678',
	DEFAULT_DATABASE = [Parcial2Jprt], -- Nombre de la base de datos actualizado
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON
GO
USE [Parcial2Jprt]
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2]
GO

DROP TABLE IF EXISTS Serie; -- Usar IF EXISTS para evitar errores si la tabla no existe
DROP PROC IF EXISTS paSerieListar; -- Usar IF EXISTS para evitar errores si el procedimiento no existe

CREATE TABLE Serie(
id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
titulo VARCHAR(250) NOT NULL,
sinopsis VARCHAR(5000) NOT NULL,
director VARCHAR(100) NOT NULL,
episodios INT NOT NULL,
fechaEstreno DATE NOT NULL,
estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado 0: Inactivo, 1: Activo
)

GO
CREATE PROC paSerieListar @parametro VARCHAR(100)
AS
  SELECT *
  FROM Serie
   WHERE estado<>-1 AND titulo+director LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY titulo ASC;

-- Nuevos valores para INSERT INTO
INSERT INTO Serie(titulo, sinopsis,director,episodios,fechaEstreno,estado)
VALUES ('The Crown', 'La serie sigue la vida de la Reina Isabel II desde la década de 1940 hasta el presente, explorando los eventos políticos y personales que han marcado su reinado y el impacto en la familia real británica.','Peter Morgan',60,'2016-11-04',1)

INSERT INTO Serie(titulo, sinopsis,director,episodios,fechaEstreno,estado)
VALUES ('Stranger Things','En la década de 1980 en Hawkins, Indiana, un grupo de amigos descubre una serie de eventos sobrenaturales y experimentos gubernamentales secretos después de que su amigo Will desaparece misteriosamente.','Duffer Brothers',34,'2016-07-15',1)

INSERT INTO Serie(titulo, sinopsis,director,episodios,fechaEstreno,estado)
VALUES ('Arcane','Ambientada en el universo de League of Legends, esta serie animada explora los orígenes de dos campeonas icónicas, Jinx y Vi, y la creciente tensión entre la rica y utópica ciudad de Piltover y la oprimida y subterránea Zaun.','Pascal Charrue',9,'2021-11-06',0)

EXEC paSerieListar 'Crown'; -- Ejemplo de ejecución con un nuevo parámetro
