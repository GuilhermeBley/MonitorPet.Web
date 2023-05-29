CREATE TABLE `dosador` (
   `IdDosador` char(36) NOT NULL,
   `Nome` varchar(255) NOT NULL,
   `ImgUrl` varchar(255) DEFAULT NULL,
   `UltimaAtualizacao` datetime DEFAULT NULL,
   `UltimaLiberacao` varchar(45) DEFAULT NULL,
   PRIMARY KEY (`IdDosador`)
 ) ENGINE=InnoDB DEFAULT CHARSET=latin1
 
 CREATE TABLE `agendamento` (
   `Id` bigint(20) NOT NULL AUTO_INCREMENT,
   `IdDosador` char(36) NOT NULL,
   `DiaSemana` int(11) NOT NULL,
   `HoraAgendada` time NOT NULL,
   `QtdeLiberadaGr` float(10,4) NOT NULL,
   `Ativado` tinyint(4) NOT NULL,
   PRIMARY KEY (`Id`),
   KEY `Pk_agendamento_id` (`IdDosador`),
   CONSTRAINT `Fk_IdDosadorAgendamento_Dosador` FOREIGN KEY (`IdDosador`) REFERENCES `dosador` (`IdDosador`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1
 
 CREATE TABLE `historicopeso` (
   `Id` bigint(20) NOT NULL AUTO_INCREMENT,
   `IdDosador` char(36) NOT NULL,
   `PesoGr` float(10,4) NOT NULL,
   `DateAt` datetime NOT NULL,
   PRIMARY KEY (`Id`),
   KEY `Fk_IdDosador_Dosador_idx` (`IdDosador`),
   CONSTRAINT `Fk_IdDosador_Dosador` FOREIGN KEY (`IdDosador`) REFERENCES `dosador` (`IdDosador`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB AUTO_INCREMENT=320 DEFAULT CHARSET=latin1
 
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
 ) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1
 
 CREATE TABLE `dosadorusuario` (
   `Id` int(11) NOT NULL AUTO_INCREMENT,
   `IdUsuario` int(11) NOT NULL,
   `IdDosador` char(36) NOT NULL,
   PRIMARY KEY (`Id`),
   UNIQUE KEY `IdUser_IdDosador_Unique` (`IdUsuario`,`IdDosador`),
   KEY `Fk_User_User_idx` (`IdUsuario`),
   KEY `Fk_Dosador_IdDosador_idx` (`IdDosador`),
   CONSTRAINT `Fk_Dosador_IdDosador` FOREIGN KEY (`IdDosador`) REFERENCES `dosador` (`IdDosador`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT `Fk_User_User` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=latin1
 
 CREATE TABLE `regraemailuser` (
   `Id` int(11) NOT NULL AUTO_INCREMENT,
   `IdUsuario` int(11) NOT NULL,
   `IdTipoEmail` int(11) NOT NULL,
   PRIMARY KEY (`Id`),
   UNIQUE KEY `IdUsuario_UNIQUE` (`IdUsuario`,`IdTipoEmail`),
   KEY `Fk_RoleEmailType_EmailType_idx` (`IdTipoEmail`),
   CONSTRAINT `Fk_RoleEmailType_EmailType` FOREIGN KEY (`IdTipoEmail`) REFERENCES `tipoemail` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
   CONSTRAINT `Fk_RoleEmailUser_User` FOREIGN KEY (`IdUsuario`) REFERENCES `usuario` (`IdUsuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
 ) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1
 
 CREATE TABLE `tipoemail` (
   `Id` int(11) NOT NULL AUTO_INCREMENT,
   `TipoEnvio` varchar(50) NOT NULL,
   `Descricao` text,
   PRIMARY KEY (`Id`),
   UNIQUE KEY `TipoEnvio_UNIQUE` (`TipoEnvio`)
 ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1

INSERT INTO monitorpet.tipoemail (TipoEnvio, Descricao) VALUES ('offline_pet', 'Aviso quando Pet está inativo.');
INSERT INTO monitorpet.tipoemail (TipoEnvio, Descricao) VALUES ('sem_alimento', 'Aviso quando Pet está sem alimento.');

INSERT INTO monitorpet.dosador (IdDosador, Nome, ImgUrl) VALUES ('ea4fa600-9ba7-4f31-a073-4c38f3078568', 'Wendy', 'https://monitorpetsa.blob.core.windows.net/files/ea4fa600-9ba7-4f31-a073-4c38f3078568.jpg');
