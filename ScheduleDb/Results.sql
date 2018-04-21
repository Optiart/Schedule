﻿CREATE TABLE [dbo].[Results]
(
	id INT IDENTITY(1,1),
	tab_id INT NOT NULL,
	result VARCHAR(max) NOT NULL,
	chain VARCHAR(max) NOT NULL,

	CONSTRAINT PK_Results PRIMARY KEY (id)
);
GO

ALTER TABLE [Results] ADD CONSTRAINT FK_Results_tab FOREIGN KEY (tab_id) REFERENCES Tabs(id) ON DELETE CASCADE;