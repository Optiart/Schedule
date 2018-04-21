CREATE TABLE [dbo].[Tabs]
(
	id INT IDENTITY(1,1),
	number_of_devices INT NOT NULL,
	device_type TINYINT NOT NULL,
	productivity VARCHAR(2048),
	number_of_palletes INT NOT NULL,
	number_of_work INT NOT NULL,
	work_per_pallete VARCHAR(max)

	CONSTRAINT PK_Tabs PRIMARY KEY (id)
);
GO