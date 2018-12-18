-- phpMyAdmin SQL Dump
-- version 4.7.9
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 18. Dez 2018 um 19:07
-- Server-Version: 10.1.31-MariaDB
-- PHP-Version: 7.2.3

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

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `timeseries`
--

CREATE TABLE `timeseries` (
  `id` int(11) NOT NULL,
  `parent_id` int(11) DEFAULT NULL,
  `name` varchar(200) NOT NULL,
  `unit` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `timeseries`
--

INSERT INTO `timeseries` (`id`, `parent_id`, `name`, `unit`) VALUES
(1, NULL, 'electric', 1),
(2, 1, 'power', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `timeseries_data`
--

CREATE TABLE `timeseries_data` (
  `id` int(11) NOT NULL,
  `delivery` datetime NOT NULL,
  `value` double NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Daten für Tabelle `timeseries_data`
--

INSERT INTO `timeseries_data` (`id`, `delivery`, `value`) VALUES
(2, '2018-12-18 00:00:00', 20),
(2, '2018-12-18 01:00:00', 25);

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `timeseries`
--
ALTER TABLE `timeseries`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `timeseries`
--
ALTER TABLE `timeseries`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
