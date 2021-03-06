CREATE DATABASE BDCITAMEDICA;
USE BDCITAMEDICA

CREATE TABLE MST_PACIENTE(
PACIENTE_CODIGO		int IDENTITY(1,1)	not null primary key,
PACIENTE_DNI		varchar(8)			not null,
PACIENTE_NOMBRE		nvarchar(120)		not null,
PACIENTE_APELLIDO	nvarchar(120)		not null,
PACIENTE_EDAD		int					not null,
PACIENTE_GENERO		char(1)				not null,
PACIENTE_ESTADO		char(1)				not null
)
CREATE INDEX IDX_PACIENTE_DNI ON dbo.MST_PACIENTE(PACIENTE_DNI);
SET IDENTITY_INSERT MST_PACIENTE ON
GO
INSERT INTO MST_PACIENTE (PACIENTE_CODIGO, PACIENTE_DNI, PACIENTE_NOMBRE, PACIENTE_APELLIDO, PACIENTE_EDAD, PACIENTE_GENERO, PACIENTE_ESTADO) VALUES (1, '08124573','Miguel Mauricio','Gomez Valverde',48,'M','A')
INSERT INTO MST_PACIENTE (PACIENTE_CODIGO, PACIENTE_DNI, PACIENTE_NOMBRE, PACIENTE_APELLIDO, PACIENTE_EDAD, PACIENTE_GENERO, PACIENTE_ESTADO) VALUES (2, '08424367','Susan Celeste','Mendoza Estrada',34,'F','A')

SET IDENTITY_INSERT MST_PACIENTE OFF

CREATE TABLE MST_ESPECIALIDAD(
ESPECIALIDAD_CODIGO int					IDENTITY(1,1)	not null primary key,
ESPECIALIDAD		nvarchar(120)		not null,
ESPECIALIDAD_ESTADO char(1)
)
SET IDENTITY_INSERT MST_ESPECIALIDAD ON
GO
insert into MST_ESPECIALIDAD (ESPECIALIDAD_CODIGO, ESPECIALIDAD, ESPECIALIDAD_ESTADO) VALUES (1,'Otorrinolaringología','A')
insert into MST_ESPECIALIDAD (ESPECIALIDAD_CODIGO, ESPECIALIDAD, ESPECIALIDAD_ESTADO) VALUES (2,'Pediatría','A')
insert into MST_ESPECIALIDAD (ESPECIALIDAD_CODIGO, ESPECIALIDAD, ESPECIALIDAD_ESTADO) VALUES (3,'Dermatología','A')

SET IDENTITY_INSERT MST_ESPECIALIDAD OFF

CREATE TABLE MST_MEDICO(
MEDICO_CODIGO		int					IDENTITY(1,1)	not null primary key,
MEDICO_DNI			varchar(8)			not null,
MEDICO_NOMBRE		nvarchar(120)		not null,
MEDICO_APELLIDO		nvarchar(120)		not null,
MEDICO_EDAD			int					not null,
MEDICO_GENERO		char(1)				not null,
MEDICO_CONSULTORIO	char(5)				not null,
MEDICO_TURNO		char(1)				not null,
MEDICO_ESPECIALIDAD int					not null FOREIGN KEY REFERENCES MST_ESPECIALIDAD(ESPECIALIDAD_CODIGO),
MEDICO_ESTADO		char(1)				not null
)
CREATE INDEX IDX_MEDICO_DNI ON dbo.MST_MEDICO(MEDICO_DNI);
SET IDENTITY_INSERT MST_MEDICO ON
GO
INSERT INTO MST_MEDICO (MEDICO_CODIGO,MEDICO_DNI, MEDICO_NOMBRE, MEDICO_APELLIDO, MEDICO_EDAD, MEDICO_GENERO, MEDICO_CONSULTORIO, MEDICO_TURNO, MEDICO_ESPECIALIDAD, MEDICO_ESTADO) VALUES (1,'08951357','Juan Manuel',' Rivera Escalante',45,'M','P1-20','M',1,'A')
INSERT INTO MST_MEDICO (MEDICO_CODIGO,MEDICO_DNI, MEDICO_NOMBRE, MEDICO_APELLIDO, MEDICO_EDAD, MEDICO_GENERO, MEDICO_CONSULTORIO, MEDICO_TURNO, MEDICO_ESPECIALIDAD, MEDICO_ESTADO) VALUES (2,'08741963','Mireya Patricia',' Pinedo Gonzales',56,'F','P2-12','T',3,'A')
SET IDENTITY_INSERT MST_MEDICO OFF
	
CREATE TABLE TB_CITA(
CITA_CODIGO			int IDENTITY(1,1)	not null primary key,
CITA_FECHA			date				not null,
CITA_HORA			time				not null,
CITA_ESPECIALIDAD	int					not null FOREIGN KEY REFERENCES MST_ESPECIALIDAD(ESPECIALIDAD_CODIGO),
CITA_MEDICO			int					not null FOREIGN KEY REFERENCES MST_MEDICO(MEDICO_CODIGO),
CITA_PACIENTE		int					not null FOREIGN KEY REFERENCES MST_PACIENTE(PACIENTE_CODIGO),
CITA_ESTADO			char(1)				not null 
)
SET IDENTITY_INSERT TB_CITA ON
GO
INSERT INTO TB_CITA (CITA_CODIGO,CITA_FECHA,CITA_HORA,CITA_ESPECIALIDAD,CITA_MEDICO,CITA_PACIENTE,CITA_ESTADO) VALUES (1,'22/12/2016','14:30:00',3,2,1,'A')
SET IDENTITY_INSERT TB_CITA OFF

GO
CREATE OR ALTER PROCEDURE SP_INSERT_PACIENTE    
    @PACIENTE_DNI			varchar(8)		,  
	@PACIENTE_NOMBRE		nvarchar(120)	,
	@PACIENTE_APELLIDO		nvarchar(120)	,
	@PACIENTE_EDAD			int				,
	@PACIENTE_GENERO		char(1)			,
	@PACIENTE_ESTADO		char(1)			,
	@PACIENTE_CODIGO		int out

AS      
	INSERT INTO MST_PACIENTE (PACIENTE_DNI,   PACIENTE_NOMBRE,  PACIENTE_APELLIDO,  PACIENTE_EDAD,  PACIENTE_GENERO, PACIENTE_ESTADO) 
					  VALUES (@PACIENTE_DNI, @PACIENTE_NOMBRE, @PACIENTE_APELLIDO, @PACIENTE_EDAD, @PACIENTE_GENERO, ISNULL(@PACIENTE_ESTADO	,'A'));    

	SELECT @PACIENTE_CODIGO = PACIENTE_CODIGO FROM MST_PACIENTE WHERE PACIENTE_DNI = @PACIENTE_DNI AND PACIENTE_NOMBRE = @PACIENTE_NOMBRE AND
																	  PACIENTE_APELLIDO = @PACIENTE_APELLIDO AND PACIENTE_EDAD = @PACIENTE_EDAD AND  PACIENTE_GENERO = @PACIENTE_GENERO
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_PACIENTE        
	@PACIENTE_DNI			varchar(8)		,  
	@PACIENTE_NOMBRE		nvarchar(120)	,
	@PACIENTE_APELLIDO		nvarchar(120)	,
	@PACIENTE_EDAD			int				,
	@PACIENTE_GENERO		char(1)			,
	@PACIENTE_ESTADO		char(1)

AS      
	DECLARE @sqlCommand nvarchar(2000)
	SET @sqlCommand = 'UPDATE MST_PACIENTE
					   SET	
					   PACIENTE_ESTADO		=	''@PACIENTE_ESTADO'' '
	IF @PACIENTE_NOMBRE		 IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',PACIENTE_NOMBRE		=	 ''@PACIENTE_NOMBRE''   '
	IF @PACIENTE_APELLIDO	 IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',PACIENTE_APELLIDO	=	 ''@PACIENTE_APELLIDO'' '
	IF @PACIENTE_EDAD		 IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',PACIENTE_EDAD		=	   @PACIENTE_EDAD 	  '
	IF @PACIENTE_GENERO		 IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',PACIENTE_GENERO		=	 ''@PACIENTE_GENERO''   '
	SET @sqlCommand = @sqlCommand  + 'WHERE PACIENTE_DNI	=	''@PACIENTE_DNI'''	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_DNI',		ISNULL(@PACIENTE_DNI		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_NOMBRE',		ISNULL(@PACIENTE_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_APELLIDO',	ISNULL(@PACIENTE_APELLIDO	,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_EDAD',		ISNULL(@PACIENTE_EDAD		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_GENERO',		ISNULL(@PACIENTE_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_ESTADO',		ISNULL(@PACIENTE_ESTADO		,'A'))	
	EXEC (@sqlCommand)
GO  

CREATE OR ALTER PROCEDURE SP_DELETE_PACIENTE        
	@PACIENTE_CODIGO		int			,
	@PACIENTE_DNI			varchar(8)		

AS   
	DECLARE @sqlMax    int
	IF @PACIENTE_CODIGO		 IS NOT NULL
		DELETE FROM MST_PACIENTE WHERE PACIENTE_CODIGO	= @PACIENTE_CODIGO
	IF @PACIENTE_DNI		 IS NOT NULL AND @PACIENTE_CODIGO IS NULL
		DELETE FROM MST_PACIENTE WHERE PACIENTE_DNI		= @PACIENTE_DNI
	SELECT  @sqlMax = MAX(PACIENTE_CODIGO) FROM MST_PACIENTE
	DBCC CHECKIDENT ('MST_PACIENTE', RESEED, @sqlMax)
GO 

CREATE OR ALTER PROCEDURE SP_LISTA_PAG_PACIENTE        
	@PACIENTE_CODIGO		int				,
	@PACIENTE_DNI			varchar(8)		,  
	@PACIENTE_NOMBRE		nvarchar(120)	,
	@PACIENTE_APELLIDO		nvarchar(120)	,
	@PACIENTE_EDAD			int				,
	@PACIENTE_GENERO		char(1)			,
	@PACIENTE_ESTADO		char(1)			,
	@ColOrder				int				,
	@NumResult				int				,
	@NumPag					int				,
	@MaxPag					int out
AS      
	DECLARE @sqlCommand  nvarchar(3000)
	DECLARE @sqlCommandC nvarchar(3000)
	DECLARE @sqlCount    int
	SET @ColOrder = ISNULL(@ColOrder, 1)
	SET @NumResult = ISNULL(@NumResult, 10)
	SET @NumPag = ISNULL(@NumPag, 1)

	SET @sqlCommand = 'SELECT {CAMPOS} FROM MST_PACIENTE
					   WHERE	
					   PACIENTE_ESTADO		=	@PACIENTE_ESTADO '
    IF @PACIENTE_CODIGO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_CODIGO		=	   @PACIENTE_CODIGO     '
	IF @PACIENTE_DNI		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_DNI			=	 ''@PACIENTE_DNI''		'
	IF @PACIENTE_NOMBRE		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_NOMBRE		=	 ''@PACIENTE_NOMBRE''	'
	IF @PACIENTE_APELLIDO	IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_APELLIDO	=	 ''@PACIENTE_APELLIDO''	'
	IF @PACIENTE_EDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_EDAD		=	   @PACIENTE_EDAD		'
	IF @PACIENTE_GENERO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_GENERO		=	 ''@PACIENTE_GENERO''   '	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_CODIGO',		ISNULL(@PACIENTE_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_DNI',		ISNULL(@PACIENTE_DNI		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_NOMBRE',		ISNULL(@PACIENTE_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_APELLIDO',	ISNULL(@PACIENTE_APELLIDO	,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_EDAD',		ISNULL(@PACIENTE_EDAD		,''))		
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_GENERO',		ISNULL(@PACIENTE_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_ESTADO',		IIF(@PACIENTE_ESTADO = '*', 'PACIENTE_ESTADO', '''' + ISNULL(@PACIENTE_ESTADO,'A') + ''''))
	SET @sqlCommandC = REPLACE(@sqlCommand, '{CAMPOS}', '@RowCount = COUNT(1)')	
	SET @sqlCommand = REPLACE(@sqlCommand, '{CAMPOS}', '*')		
	EXECUTE sp_executesql @sqlCommandC, N'@RowCount int OUTPUT', @RowCount = @sqlCount OUTPUT
	SET @MaxPag = CEILING(CAST(@sqlCount AS FLOAT) / @NumResult)
	SET @sqlCommand = 'SELECT A.* FROM(' + @sqlCommand +')A ORDER BY @ColOrder OFFSET ' + CAST(((@NumPag - 1) * @NumResult) AS VARCHAR) + ' ROWS FETCH NEXT ' + CAST(@NumResult AS VARCHAR) + ' ROWS ONLY'
	SET @sqlCommand = REPLACE(@sqlCommand, '@ColOrder', @ColOrder )		
	EXEC (@sqlCommand)
GO 

CREATE OR ALTER PROCEDURE SP_LISTA_PACIENTE        
	@PACIENTE_CODIGO		int				,
	@PACIENTE_DNI			varchar(8)		,  
	@PACIENTE_NOMBRE		nvarchar(120)	,
	@PACIENTE_APELLIDO		nvarchar(120)	,
	@PACIENTE_EDAD			int				,
	@PACIENTE_GENERO		char(1)			,
	@PACIENTE_ESTADO		char(1)			,
	@ColOrder				int				
AS      
	DECLARE @sqlCommand  nvarchar(3000)		
	SET @ColOrder = ISNULL(@ColOrder, 1)	

	SET @sqlCommand = 'SELECT PACIENTE_CODIGO, PACIENTE_DNI, PACIENTE_NOMBRE, PACIENTE_APELLIDO, PACIENTE_EDAD, PACIENTE_GENERO, PACIENTE_ESTADO  FROM MST_PACIENTE
					   WHERE	
					   PACIENTE_ESTADO		=	@PACIENTE_ESTADO '
    IF @PACIENTE_CODIGO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_CODIGO		=	   @PACIENTE_CODIGO     '
	IF @PACIENTE_DNI		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_DNI			=	 ''@PACIENTE_DNI''		'
	IF @PACIENTE_NOMBRE		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_NOMBRE		=	 ''@PACIENTE_NOMBRE''	'
	IF @PACIENTE_APELLIDO	IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_APELLIDO	=	 ''@PACIENTE_APELLIDO''	'
	IF @PACIENTE_EDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_EDAD		=	   @PACIENTE_EDAD		'
	IF @PACIENTE_GENERO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND PACIENTE_GENERO		=	 ''@PACIENTE_GENERO''   '	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_CODIGO',		ISNULL(@PACIENTE_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_DNI',		ISNULL(@PACIENTE_DNI		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_NOMBRE',		ISNULL(@PACIENTE_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_APELLIDO',	ISNULL(@PACIENTE_APELLIDO	,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_EDAD',		ISNULL(@PACIENTE_EDAD		,''))		
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_GENERO',		ISNULL(@PACIENTE_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@PACIENTE_ESTADO',		IIF(@PACIENTE_ESTADO = '*', 'PACIENTE_ESTADO', '''' + ISNULL(@PACIENTE_ESTADO,'A') + ''''))
	SET @sqlCommand = @sqlCommand + ' ORDER BY @ColOrder'
	SET @sqlCommand = REPLACE(@sqlCommand, '@ColOrder', @ColOrder )		
	EXEC (@sqlCommand)
GO 

/*MST_MEDICO*/

CREATE OR ALTER PROCEDURE SP_INSERT_MEDICO   
	@MEDICO_DNI				varchar(8)		,
	@MEDICO_NOMBRE			nvarchar(120)	,
	@MEDICO_APELLIDO		nvarchar(120)	,
	@MEDICO_EDAD			int				,
	@MEDICO_GENERO			char(1)			,
	@MEDICO_CONSULTORIO		char(5)			,
	@MEDICO_TURNO			char(1)			,
	@MEDICO_ESPECIALIDAD	int				,
	@MEDICO_ESTADO			char(1)			,
	@MEDICO_CODIGO			int	out			
AS      
	INSERT INTO MST_MEDICO ( MEDICO_DNI,  MEDICO_NOMBRE,  MEDICO_APELLIDO,  MEDICO_EDAD,  MEDICO_GENERO,  MEDICO_CONSULTORIO,  MEDICO_TURNO,  MEDICO_ESPECIALIDAD, MEDICO_ESTADO) 
					VALUES (@MEDICO_DNI, @MEDICO_NOMBRE, @MEDICO_APELLIDO, @MEDICO_EDAD, @MEDICO_GENERO, @MEDICO_CONSULTORIO, @MEDICO_TURNO, @MEDICO_ESPECIALIDAD, ISNULL(@MEDICO_ESTADO	,'A'));    

	SELECT @MEDICO_CODIGO = MEDICO_CODIGO FROM MST_MEDICO WHERE MEDICO_DNI = @MEDICO_DNI AND MEDICO_NOMBRE = @MEDICO_NOMBRE AND MEDICO_APELLIDO = @MEDICO_APELLIDO AND 
																MEDICO_EDAD = @MEDICO_EDAD AND  MEDICO_GENERO = @MEDICO_GENERO AND MEDICO_CONSULTORIO = @MEDICO_CONSULTORIO AND 
																MEDICO_TURNO = @MEDICO_TURNO AND MEDICO_ESPECIALIDAD = @MEDICO_ESPECIALIDAD
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_MEDICO        
	@MEDICO_CODIGO			int				,
	@MEDICO_DNI				varchar(8)		,
	@MEDICO_NOMBRE			nvarchar(120)	,
	@MEDICO_APELLIDO		nvarchar(120)	,
	@MEDICO_EDAD			int				,
	@MEDICO_GENERO			char(1)			,
	@MEDICO_CONSULTORIO		char(5)			,
	@MEDICO_TURNO			char(1)			,
	@MEDICO_ESPECIALIDAD	int				,
	@MEDICO_ESTADO			char(1)			
AS      
	DECLARE @sqlCommand nvarchar(3000)
	SET @sqlCommand = 'UPDATE MST_MEDICO
					   SET	
					   MEDICO_ESTADO		=	''@MEDICO_ESTADO'' '
	IF @MEDICO_DNI				IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_DNI			=	 ''@MEDICO_DNI''		 '
	IF @MEDICO_NOMBRE			IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_NOMBRE		=	 ''@MEDICO_NOMBRE''		 '
	IF @MEDICO_APELLIDO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_APELLIDO		=	 ''@MEDICO_APELLIDO''	 '
	IF @MEDICO_EDAD				IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_EDAD			=	   @MEDICO_EDAD			 '
	IF @MEDICO_GENERO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_GENERO		=	 ''@MEDICO_GENERO''		 '
	IF @MEDICO_CONSULTORIO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_CONSULTORIO	=	 ''@MEDICO_CONSULTORIO'' '
	IF @MEDICO_TURNO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_TURNO		=	 ''@MEDICO_TURNO''		 '
	IF @MEDICO_ESPECIALIDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ ',MEDICO_ESPECIALIDAD	=	   @MEDICO_ESPECIALIDAD  '	
	SET @sqlCommand = @sqlCommand  + 'WHERE MEDICO_CODIGO	=	@MEDICO_CODIGO'	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CODIGO',		ISNULL(@MEDICO_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_DNI',			ISNULL(@MEDICO_DNI			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_NOMBRE',		ISNULL(@MEDICO_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_APELLIDO',		ISNULL(@MEDICO_APELLIDO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_EDAD',			ISNULL(@MEDICO_EDAD			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_GENERO',		ISNULL(@MEDICO_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CONSULTORIO',	ISNULL(@MEDICO_CONSULTORIO	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_TURNO',		ISNULL(@MEDICO_TURNO		,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESPECIALIDAD',	ISNULL(@MEDICO_ESPECIALIDAD	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESTADO',		ISNULL(@MEDICO_ESTADO		,'A'))	
	EXEC (@sqlCommand)
GO  

CREATE OR ALTER PROCEDURE SP_DELETE_MEDICO       
	@MEDICO_CODIGO		int			,
	@MEDICO_DNI			varchar(8)		

AS      
	DECLARE @sqlMax    int
	IF @MEDICO_CODIGO	IS NOT NULL
		DELETE FROM MST_MEDICO WHERE MEDICO_CODIGO	= @MEDICO_CODIGO
	IF @MEDICO_DNI		IS NOT NULL AND @MEDICO_CODIGO IS NULL
		DELETE FROM MST_MEDICO WHERE MEDICO_DNI		= @MEDICO_DNI
	SELECT  @sqlMax = MAX(MEDICO_CODIGO) FROM MST_MEDICO
	DBCC CHECKIDENT ('MST_MEDICO', RESEED, @sqlMax)
GO 

CREATE OR ALTER PROCEDURE SP_LISTA_PAG_MEDICO       
	@MEDICO_CODIGO			int				,
	@MEDICO_DNI				varchar(8)		,
	@MEDICO_NOMBRE			nvarchar(120)	,
	@MEDICO_APELLIDO		nvarchar(120)	,
	@MEDICO_EDAD			int				,
	@MEDICO_GENERO			char(1)			,
	@MEDICO_CONSULTORIO		char(5)			,
	@MEDICO_TURNO			char(1)			,
	@MEDICO_ESPECIALIDAD	int				,
	@MEDICO_ESTADO			char(1)			,
	@ColOrder				int				,
	@NumResult				int				,
	@NumPag					int				,
	@MaxPag					int out
AS      
	DECLARE @sqlCommand  nvarchar(3000)
	DECLARE @sqlCommandC nvarchar(3000)
	DECLARE @sqlCount    int
	SET @ColOrder = ISNULL(@ColOrder, 1)
	SET @NumResult = ISNULL(@NumResult, 10)
	SET @NumPag = ISNULL(@NumPag, 1)

	SET @sqlCommand = 'SELECT {CAMPOS} FROM MST_MEDICO M {JOIN}
					   WHERE	
					   M.MEDICO_ESTADO		=	@MEDICO_ESTADO '
	IF @MEDICO_CODIGO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_CODIGO		=	   @MEDICO_CODIGO		 '
    IF @MEDICO_DNI				IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_DNI			=	 ''@MEDICO_DNI''		 '
	IF @MEDICO_NOMBRE			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_NOMBRE		=	 ''@MEDICO_NOMBRE''		 '
	IF @MEDICO_APELLIDO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_APELLIDO	=	 ''@MEDICO_APELLIDO''	 '
	IF @MEDICO_EDAD				IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_EDAD		=	   @MEDICO_EDAD			 '
	IF @MEDICO_GENERO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_GENERO		=	 ''@MEDICO_GENERO''		 '
	IF @MEDICO_CONSULTORIO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_CONSULTORIO	=	 ''@MEDICO_CONSULTORIO'' '
	IF @MEDICO_TURNO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_TURNO		=	 ''@MEDICO_TURNO''		 '
	IF @MEDICO_ESPECIALIDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_ESPECIALIDAD=	   @MEDICO_ESPECIALIDAD  '		
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CODIGO',		ISNULL(@MEDICO_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_DNI',			ISNULL(@MEDICO_DNI			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_NOMBRE',		ISNULL(@MEDICO_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_APELLIDO',		ISNULL(@MEDICO_APELLIDO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_EDAD',			ISNULL(@MEDICO_EDAD			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_GENERO',		ISNULL(@MEDICO_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CONSULTORIO',	ISNULL(@MEDICO_CONSULTORIO	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_TURNO',		ISNULL(@MEDICO_TURNO		,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESPECIALIDAD',	ISNULL(@MEDICO_ESPECIALIDAD	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESTADO',		IIF(@MEDICO_ESTADO = '*', 'M.MEDICO_ESTADO', '''' + ISNULL(@MEDICO_ESTADO,'A') + ''''))
	SET @sqlCommandC = REPLACE(@sqlCommand, '{CAMPOS}', '@RowCount = COUNT(1)')	
	SET @sqlCommandC = REPLACE(@sqlCommandC, '{JOIN}', '')	
	SET @sqlCommand = REPLACE(@sqlCommand, '{CAMPOS}', 'M.*, E.ESPECIALIDAD')		
	SET @sqlCommand = REPLACE(@sqlCommand, '{JOIN}', 'LEFT JOIN MST_ESPECIALIDAD E ON M.MEDICO_ESPECIALIDAD = E.ESPECIALIDAD_CODIGO AND E.ESPECIALIDAD_ESTADO = ''A''')		
	EXECUTE sp_executesql @sqlCommandC, N'@RowCount int OUTPUT', @RowCount = @sqlCount OUTPUT
	SET @MaxPag = CEILING(CAST(@sqlCount AS FLOAT) / @NumResult)
	SET @sqlCommand = 'SELECT A.* FROM(' + @sqlCommand +')A ORDER BY @ColOrder OFFSET ' + CAST(((@NumPag - 1) * @NumResult) AS VARCHAR) + ' ROWS FETCH NEXT ' + CAST(@NumResult AS VARCHAR) + ' ROWS ONLY'
	SET @sqlCommand = REPLACE(@sqlCommand, '@ColOrder', @ColOrder )			
	EXEC (@sqlCommand)
GO 

CREATE OR ALTER PROCEDURE SP_LISTA_MEDICO        
	@MEDICO_CODIGO			int				,
	@MEDICO_DNI				varchar(8)		,
	@MEDICO_NOMBRE			nvarchar(120)	,
	@MEDICO_APELLIDO		nvarchar(120)	,
	@MEDICO_EDAD			int				,
	@MEDICO_GENERO			char(1)			,
	@MEDICO_CONSULTORIO		char(5)			,
	@MEDICO_TURNO			char(1)			,
	@MEDICO_ESPECIALIDAD	int				,
	@MEDICO_ESTADO			char(1)			,
	@ColOrder				int				
AS      
	DECLARE @sqlCommand  nvarchar(3000)		
	SET @ColOrder = ISNULL(@ColOrder, 1)	

	SET @sqlCommand = 'SELECT M.*, E.ESPECIALIDAD FROM MST_MEDICO M 
					   LEFT JOIN MST_ESPECIALIDAD E ON M.MEDICO_ESPECIALIDAD = E.ESPECIALIDAD_CODIGO AND E.ESPECIALIDAD_ESTADO = ''A''
					   WHERE	
					   M.MEDICO_ESTADO		=	@MEDICO_ESTADO '
	IF @MEDICO_CODIGO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_CODIGO		=	   @MEDICO_CODIGO		 '
    IF @MEDICO_DNI				IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_DNI			=	 ''@MEDICO_DNI''		 '
	IF @MEDICO_NOMBRE			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_NOMBRE		=	 ''@MEDICO_NOMBRE''		 '
	IF @MEDICO_APELLIDO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_APELLIDO	=	 ''@MEDICO_APELLIDO''	 '
	IF @MEDICO_EDAD				IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_EDAD		=	   @MEDICO_EDAD			 '
	IF @MEDICO_GENERO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_GENERO		=	 ''@MEDICO_GENERO''		 '
	IF @MEDICO_CONSULTORIO		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_CONSULTORIO	=	 ''@MEDICO_CONSULTORIO'' '
	IF @MEDICO_TURNO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_TURNO		=	 ''@MEDICO_TURNO''		 '
	IF @MEDICO_ESPECIALIDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND M.MEDICO_ESPECIALIDAD=	   @MEDICO_ESPECIALIDAD  '			
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CODIGO',		ISNULL(@MEDICO_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_DNI',			ISNULL(@MEDICO_DNI			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_NOMBRE',		ISNULL(@MEDICO_NOMBRE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_APELLIDO',		ISNULL(@MEDICO_APELLIDO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_EDAD',			ISNULL(@MEDICO_EDAD			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_GENERO',		ISNULL(@MEDICO_GENERO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_CONSULTORIO',	ISNULL(@MEDICO_CONSULTORIO	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_TURNO',		ISNULL(@MEDICO_TURNO		,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESPECIALIDAD',	ISNULL(@MEDICO_ESPECIALIDAD	,''))
	SET @sqlCommand = REPLACE(@sqlCommand,  '@MEDICO_ESTADO',		IIF(@MEDICO_ESTADO = '*', 'M.MEDICO_ESTADO', '''' + ISNULL(@MEDICO_ESTADO,'A') + ''''))
	SET @sqlCommand = @sqlCommand + ' ORDER BY @ColOrder'
	SET @sqlCommand = REPLACE(@sqlCommand, '@ColOrder', @ColOrder )			
	EXEC (@sqlCommand)
GO 

/*MST_ESPECIALIDAD*/
CREATE OR ALTER PROCEDURE SP_LISTA_ESPECIALIDAD       
	@ESPECIALIDAD_CODIGO	int				,
	@ESPECIALIDAD			nvarchar(120)	,
	@ESPECIALIDAD_ESTADO	char(1)			
AS      
	DECLARE @sqlCommand  nvarchar(3000)		
	SET @sqlCommand = 'SELECT * FROM MST_ESPECIALIDAD 
					   WHERE	
					   ESPECIALIDAD_ESTADO		=	@ESPECIALIDAD_ESTADO '
	IF @ESPECIALIDAD_CODIGO	IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND ESPECIALIDAD_CODIGO	=	   @ESPECIALIDAD_CODIGO	'
    IF @ESPECIALIDAD		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND ESPECIALIDAD			=	 ''@ESPECIALIDAD''	'	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@ESPECIALIDAD_ESTADO',		IIF(@ESPECIALIDAD_ESTADO = '*', 'ESPECIALIDAD_ESTADO', '''' + ISNULL(@ESPECIALIDAD_ESTADO,'A') + ''''))						
	SET @sqlCommand = REPLACE(@sqlCommand,  '@ESPECIALIDAD_CODIGO',		ISNULL(@ESPECIALIDAD_CODIGO		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@ESPECIALIDAD',			ISNULL(@ESPECIALIDAD			,''))			
	EXEC (@sqlCommand)
GO 

/*TB_CITA*/
CREATE OR ALTER PROCEDURE SP_INSERT_CITA        
	@CITA_FECHA			varchar(20)	,
	@CITA_HORA			varchar(20)	,
	@CITA_ESPECIALIDAD	int			,
	@CITA_MEDICO		int			,
	@CITA_PACIENTE		int			,
	@CITA_ESTADO		char(1)		,
	@CITA_CODIGO		int out
AS      
	INSERT INTO TB_CITA( CITA_FECHA,  CITA_HORA,  CITA_ESPECIALIDAD,  CITA_MEDICO,  CITA_PACIENTE, CITA_ESTADO) 
				VALUES (@CITA_FECHA, @CITA_HORA, @CITA_ESPECIALIDAD, @CITA_MEDICO, @CITA_PACIENTE, ISNULL(@CITA_ESTADO	,'A'));    

	SELECT @CITA_CODIGO = CITA_CODIGO FROM TB_CITA WHERE CITA_FECHA = @CITA_FECHA AND CITA_HORA = @CITA_HORA AND CITA_ESPECIALIDAD = @CITA_ESPECIALIDAD AND 
														 CITA_MEDICO = @CITA_MEDICO AND CITA_PACIENTE = @CITA_PACIENTE
GO

CREATE OR ALTER PROCEDURE SP_LISTA_PAG_CITA       
	@CITA_CODIGO		int 		,
	@CITA_FECHA			varchar(20)	,
	@CITA_HORA			varchar(20)	,
	@CITA_ESPECIALIDAD	int			,
	@CITA_MEDICO		int			,
	@CITA_PACIENTE		int			,
	@CITA_ESTADO		char(1)		,	
	@ColOrder			int			,
	@NumResult			int			,
	@NumPag				int			,
	@MaxPag				int out
AS      
	DECLARE @sqlCommand  nvarchar(3000)
	DECLARE @sqlCommandC nvarchar(3000)
	DECLARE @sqlCount    int
	SET @ColOrder = ISNULL(@ColOrder, 1)
	SET @NumResult = ISNULL(@NumResult, 10)
	SET @NumPag = ISNULL(@NumPag, 1)

	SET @sqlCommand = 'SELECT {CAMPOS} FROM TB_CITA C {JOIN}
					   WHERE	
					   C.CITA_ESTADO		=	@CITA_ESTADO '
	IF @CITA_CODIGO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_CODIGO		=	   @CITA_CODIGO			'
    IF @CITA_FECHA			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_FECHA			=	 ''@CITA_FECHA''		'
	IF @CITA_HORA			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_HORA			=	 ''@CITA_HORA''			'
	IF @CITA_ESPECIALIDAD	IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_ESPECIALIDAD	=	 ''@CITA_ESPECIALIDAD''	'
	IF @CITA_MEDICO			IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_MEDICO		=	   @CITA_MEDICO			'
	IF @CITA_PACIENTE		IS NOT NULL SET @sqlCommand = @sqlCommand	+ 'AND C.CITA_PACIENTE		=	 ''@CITA_PACIENTE''		'	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_CODIGO',			ISNULL(@CITA_CODIGO			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_FECHA',			ISNULL(@CITA_FECHA			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_HORA',			ISNULL(@CITA_HORA			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_ESPECIALIDAD',	ISNULL(@CITA_ESPECIALIDAD	,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_MEDICO',			ISNULL(@CITA_MEDICO			,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_PACIENTE',		ISNULL(@CITA_PACIENTE		,''))	
	SET @sqlCommand = REPLACE(@sqlCommand,  '@CITA_ESTADO',			IIF(@CITA_ESTADO = '*', 'C.CITA_ESTADO', '''' + ISNULL(@CITA_ESTADO,'A') + ''''))	
	SET @sqlCommandC = REPLACE(@sqlCommand, '{CAMPOS}', '@RowCount = COUNT(1)')	
	SET @sqlCommandC = REPLACE(@sqlCommandC,'{JOIN}', '')	
	SET @sqlCommand = REPLACE(@sqlCommand, '{CAMPOS}', 'C.CITA_CODIGO, CONVERT(VARCHAR, C.CITA_FECHA, 3) AS CITA_FECHA, SUBSTRING(CONVERT(VARCHAR, C.CITA_HORA, 8),1,5) AS CITA_HORA, C.CITA_ESPECIALIDAD, C.CITA_MEDICO, C.CITA_PACIENTE, C.CITA_ESTADO,
														E.ESPECIALIDAD, P.PACIENTE_DNI, P.PACIENTE_NOMBRE, P.PACIENTE_APELLIDO, M.MEDICO_DNI, M.MEDICO_NOMBRE, M.MEDICO_APELLIDO, M.MEDICO_CONSULTORIO, M.MEDICO_TURNO')		
	SET @sqlCommand = REPLACE(@sqlCommand, '{JOIN}', 'LEFT JOIN MST_ESPECIALIDAD E ON E.ESPECIALIDAD_CODIGO = C.CITA_ESPECIALIDAD AND E.ESPECIALIDAD_ESTADO = ''A''
													  LEFT JOIN MST_PACIENTE	 P ON P.PACIENTE_CODIGO = C.CITA_PACIENTE AND P.PACIENTE_ESTADO = ''A''
													  LEFT JOIN MST_MEDICO		 M ON M.MEDICO_CODIGO = C.CITA_MEDICO AND M.MEDICO_ESTADO = ''A''')		
	EXECUTE sp_executesql @sqlCommandC, N'@RowCount int OUTPUT', @RowCount = @sqlCount OUTPUT
	SET @MaxPag = CEILING(CAST(@sqlCount AS FLOAT) / @NumResult)
	SET @sqlCommand = 'SELECT A.* FROM(' + @sqlCommand +')A ORDER BY @ColOrder OFFSET ' + CAST(((@NumPag - 1) * @NumResult) AS VARCHAR) + ' ROWS FETCH NEXT ' + CAST(@NumResult AS VARCHAR) + ' ROWS ONLY'
	SET @sqlCommand = REPLACE(@sqlCommand, '@ColOrder', @ColOrder )					
	PRINT @sqlCommand
	EXEC (@sqlCommand)
GO