-- phpMyAdmin SQL Dump
-- version 4.8.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 23. Dez 2018 um 16:31
-- Server-Version: 10.1.32-MariaDB
-- PHP-Version: 7.2.5

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `timeseries`
--
CREATE DATABASE IF NOT EXISTS `timeseries` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `timeseries`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `timeseries`
--

DROP TABLE IF EXISTS `timeseries`;
CREATE TABLE `timeseries` (
  `id` int(11) NOT NULL,
  `name` varchar(700) NOT NULL,
  `unit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `timeseries`
--

INSERT INTO `timeseries` (`id`, `name`, `unit`) VALUES
(2, 'test.data', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `timeseries_data`
--

DROP TABLE IF EXISTS `timeseries_data`;
CREATE TABLE `timeseries_data` (
  `id` int(11) NOT NULL,
  `delivery` datetime NOT NULL,
  `value` double NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `timeseries_data`
--

INSERT INTO `timeseries_data` (`id`, `delivery`, `value`) VALUES
(2, '2018-12-18 23:00:00', 24),
(2, '2018-12-18 22:00:00', 23),
(2, '2018-12-18 21:00:00', 22),
(2, '2018-12-18 20:00:00', 21),
(2, '2018-12-18 19:00:00', 20),
(2, '2018-12-18 18:00:00', 19),
(2, '2018-12-18 17:00:00', 18),
(2, '2018-12-18 16:00:00', 17),
(2, '2018-12-18 15:00:00', 16),
(2, '2018-12-18 14:00:00', 15),
(2, '2018-12-18 13:00:00', 14),
(2, '2018-12-18 12:00:00', 13),
(2, '2018-12-18 11:00:00', 12),
(2, '2018-12-18 10:00:00', 11),
(2, '2018-12-18 09:00:00', 10),
(2, '2018-12-18 08:00:00', 9),
(2, '2018-12-18 07:00:00', 8),
(2, '2018-12-18 06:00:00', 7),
(2, '2018-12-18 05:00:00', 6),
(2, '2018-12-18 04:00:00', 5),
(2, '2018-12-18 03:00:00', 4),
(2, '2018-12-18 02:00:00', 3),
(2, '2018-12-18 01:00:00', 2),
(2, '2018-12-18 00:00:00', 1);

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `timeseries`
--
ALTER TABLE `timeseries`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `timeseries_data`
--
ALTER TABLE `timeseries_data`
  ADD PRIMARY KEY (`id`,`delivery`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `timeseries`
--
ALTER TABLE `timeseries`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
