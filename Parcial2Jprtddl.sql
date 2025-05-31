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


EXEC paSerieListar 'Dark'; -- Ejemplo de ejecución con un nuevo parámetro


ALTER TABLE Serie
ADD urlTrailer VARCHAR(500);


ALTER TABLE Serie
ADD idiomaOriginal VARCHAR(50);


select * from Serie;

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlTrailer, idiomaOriginal)
VALUES (
    'Severance',
    'Mark Scout lidera un equipo en Lumon Industries, donde los empleados se someten a un procedimiento quirúrgico que separa sus recuerdos entre su vida laboral y personal. Esta arriesgada experiencia pone a prueba la verdadera naturaleza de su trabajo.',
    'Ben Stiller',
    9,
    '2022-02-18',
    1,
    'https://www.youtube.com', 
    'Inglés'
);

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlTrailer, idiomaOriginal)
VALUES (
    'Dark',
    'Cuando dos niños desaparecen en un pequeño pueblo alemán, sus enigmáticas relaciones salen a la luz entre cuatro familias mientras desvelan una conspiración de viaje en el tiempo que abarca varias generaciones.',
    'Baran bo Odar',
    26,
    '2017-12-01',
    1,
    'https://www.youtube.com',
    'Alemán'
);


EXEC paSerieListar 'Dark'; -- Ejemplo de ejecución con un nuevo parámetro
