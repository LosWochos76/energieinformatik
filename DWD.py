import urllib.request
import re
from datetime import datetime
from collections import namedtuple

Station = namedtuple("Station", "id von bis hoehe geo_breite geo_laenge")
base_url = 'https://opendata.dwd.de/climate_environment/CDC/observations_germany/climate/daily/kl/'
response = urllib.request.urlopen(base_url + 'recent/KL_Tageswerte_Beschreibung_Stationen.txt')
data = response.read().decode('windows-1252')
lines = re.split(r'\n', data)[2:-1]

for line in lines:
  station = Station(
    id = int(line[0:5].strip()),
    von = datetime.strptime(line[6:14].strip(), '%Y%m%d'),
    bis = datetime.strptime(line[15:23].strip(), '%Y%m%d'),
    hoehe = float(line[25:39].strip()),
    geo_breite = float(line[39:50].strip()),
    geo_laenge = float(line[51:60].strip())
  )
  
  
