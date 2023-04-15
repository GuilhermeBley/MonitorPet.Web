CREATE TABLE `usuario` (
   `IdUsuario` int(11) NOT NULL AUTO_INCREMENT,
   `Nome` varchar(255) NOT NULL,
   `Apelido` varchar(255) NOT NULL,
   `Email` varchar(255) NOT NULL,
   `EmailConfirmado` tinyint(1) DEFAULT NULL,
   `Login` varchar(255) NOT NULL,
   `SenhaHash` varchar(500) DEFAULT NULL,
   `SenhaSalt` varchar(255) DEFAULT NULL,
   `BloqueadoAte` datetime DEFAULT NULL,
   `ContagemErros` int(11) DEFAULT NULL,
   PRIMARY KEY (`IdUsuario`),
   UNIQUE KEY `Login_UNIQUE` (`Login`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1

CREATE TABLE `dosador` (
   `IdDosador` char(36) NOT NULL,
   `Nome` varchar(255) NOT NULL,
   `PesoMaxGr` float(7,4) DEFAULT NULL,
   PRIMARY KEY (`IdDosador`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1

CREATE TABLE `dosadorusuario` (
   `Id` int(11) NOT NULL,
   `IdUsuario` int(11) NOT NULL,
   `IdDosador` char(36) NOT NULL,
   PRIMARY KEY (`Id`),
   UNIQUE KEY `IdUser_IdDosador_Unique` (`IdUsuario`,`IdDosador`),
   KEY `Fk_User_User_idx` (`IdUsuario`),
   KEY `Fk_Dosador_IdDosador_idx` (`IdDosador`),
   CONSTRAINT `Fk_Dosador_IdDosador` FOREIGN KEY (`IdDosador`) REFERENCES `dosador` (`IdDosador`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT `Fk_User_User` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1

CREATE TABLE `Peso` (
  `IdPeso` long auto_increment,
  `IdDosador` char(36) not null,
  `Peso` float(7,4) not null,
  `DataRegistro` datetime not null,
  PRIMARY KEY (`IdPeso`),
  FOREIGN KEY (`IdDosador`) REFERENCES `Dosador`(`IdDosador`)
);

CREATE TABLE `Agendamento` (
  `IdAgendamento` int auto_increment,
  `IdDosador` char(36) not null,
  `DiaSemana` int,
  `DataAgendada` datetime,
  `QtdeLiberada` float(7,4) not null,
  PRIMARY KEY (`IdAgendamento`),
  FOREIGN KEY (`IdDosador`) REFERENCES `Dosador`(`IdDosador`)
);

CREATE TABLE `Endereço` (
  `IdVeterinario` int not null,
  `Logradouro` varchar(255),
  `Numero` int,
  `Cidade` varchar(255),
  `Estado` varchar(255),
  `Complemento` varchar(255),
  PRIMARY KEY (`IdVeterinario`)
);

CREATE TABLE `Veterinário` (
  `IdVeterinario` int auto_increment,
  `IdDosador` char(36) not null,
  `Nome` varchar(255) not null,
  `Email` varchar(255),
  `Celular` varchar(15),
  PRIMARY KEY (`IdVeterinario`)
);