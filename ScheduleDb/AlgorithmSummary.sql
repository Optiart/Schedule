
CREATE TABLE AlgorithmSummary
(
	id INT IDENTITY(1,1),
	result_id INT NOT NULL,
	type TINYINT NOT NULL,
	c_star DECIMAL NOT NULL,
	c_max DECIMAL NOT NULL

	CONSTRAINT PK_AlgorithmSummary PRIMARY KEY,
	CONSTRAINT FK_AlgorithmSummary_result FOREIGN KEY (result_id) REFERENCES Results(id) ON DELETE CASCADE
);