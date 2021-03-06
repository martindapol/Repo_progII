USE [recetas_db]
GO
/****** Object:  Table [dbo].[Recetas]    Script Date: 10/24/2021 10:11:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Recetas](
	[id_receta] [int] NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[cheff] [varchar](100) NULL,
	[tipo_receta] [int] NOT NULL,
 CONSTRAINT [PK_Recetas] PRIMARY KEY CLUSTERED 
(
	[id_receta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (2, N'TEST', NULL, 4)
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (3, N'TEST ', NULL, 1)
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (4, N'SOPA TEST', NULL, 1)
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (5, N'Receta 05', NULL, 3)
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (6, N'Ultima receta 06', NULL, 3)
INSERT [dbo].[Recetas] ([id_receta], [nombre], [cheff], [tipo_receta]) VALUES (99, N'Receta TEST', N'Cheff TEST', 1)
/****** Object:  Table [dbo].[Ingredientes]    Script Date: 10/24/2021 10:11:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ingredientes](
	[id_ingrediente] [int] NOT NULL,
	[n_ingrediente] [varchar](50) NOT NULL,
	[unidad_medida] [varchar](50) NULL,
 CONSTRAINT [PK_Ingredientes] PRIMARY KEY CLUSTERED 
(
	[id_ingrediente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (1, N'Sal', N'gramos')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (2, N'Pimienta', N'gramos')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (3, N'Agua', N'cm3')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (4, N'Aceite', N'cm3')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (5, N'carne', N'gramos')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (6, N'caldo', N'cm3')
INSERT [dbo].[Ingredientes] ([id_ingrediente], [n_ingrediente], [unidad_medida]) VALUES (7, N'Azucar', N'gramos')
/****** Object:  Table [dbo].[Detalles_Receta]    Script Date: 10/24/2021 10:11:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detalles_Receta](
	[id_receta] [int] NOT NULL,
	[id_ingrediente] [int] NOT NULL,
	[cantidad] [numeric](5, 2) NOT NULL,
 CONSTRAINT [PK_Detalles_Receta] PRIMARY KEY CLUSTERED 
(
	[id_receta] ASC,
	[id_ingrediente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (2, 3, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (2, 4, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (3, 2, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (3, 4, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (4, 1, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (4, 6, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (5, 2, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (5, 4, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (6, 4, CAST(1.00 AS Numeric(5, 2)))
INSERT [dbo].[Detalles_Receta] ([id_receta], [id_ingrediente], [cantidad]) VALUES (99, 5, CAST(1.00 AS Numeric(5, 2)))
/****** Object:  StoredProcedure [dbo].[SP_CONSULTAR_INGREDIENTES]    Script Date: 10/24/2021 10:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CONSULTAR_INGREDIENTES]
AS
BEGIN
	
	SELECT * from Ingredientes ORDER BY 2;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_PROXIMO_ID]    Script Date: 10/24/2021 10:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_PROXIMO_ID]
@next int OUTPUT
AS
BEGIN
	SET @next = (SELECT COUNT(*)+1  FROM Recetas);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERTAR_RECETA]    Script Date: 10/24/2021 10:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_INSERTAR_RECETA] 
	@id_receta int, 
	@tipo_receta int,
	@nombre varchar(50),
	@cheff varchar(100)
AS
BEGIN
	INSERT INTO Recetas (id_receta, nombre, cheff , tipo_receta)
    VALUES (@id_receta, @nombre, @cheff, @tipo_receta );

END
GO
/****** Object:  StoredProcedure [dbo].[SP_INSERTAR_DETALLES]    Script Date: 10/24/2021 10:11:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_INSERTAR_DETALLES] 
	@id_receta int,
	@id_ingrediente int, 
	@cantidad int
AS
BEGIN
	INSERT INTO Detalles_Receta(id_receta,id_ingrediente,cantidad)
    VALUES (@id_receta, @id_ingrediente, @cantidad);
  
END
GO
/****** Object:  ForeignKey [FK_Detalles_Receta_Ingredientes]    Script Date: 10/24/2021 10:11:33 ******/
ALTER TABLE [dbo].[Detalles_Receta]  WITH CHECK ADD  CONSTRAINT [FK_Detalles_Receta_Ingredientes] FOREIGN KEY([id_ingrediente])
REFERENCES [dbo].[Ingredientes] ([id_ingrediente])
GO
ALTER TABLE [dbo].[Detalles_Receta] CHECK CONSTRAINT [FK_Detalles_Receta_Ingredientes]
GO
/****** Object:  ForeignKey [FK_Ingredientes_Ingredientes]    Script Date: 10/24/2021 10:11:33 ******/
ALTER TABLE [dbo].[Ingredientes]  WITH CHECK ADD  CONSTRAINT [FK_Ingredientes_Ingredientes] FOREIGN KEY([id_ingrediente])
REFERENCES [dbo].[Ingredientes] ([id_ingrediente])
GO
ALTER TABLE [dbo].[Ingredientes] CHECK CONSTRAINT [FK_Ingredientes_Ingredientes]
GO
