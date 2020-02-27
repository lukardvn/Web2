import requests
import Vehicle
import Station
import HaversineFormula

baseUrl = "http://localhost:52295/api/"

class Line():
    allLinesCache = {}
    path = None
    busStops = None
    buses = None
    LineId = None

    def __init__(self,lineId,path,busStops,buses):
        self.path = path
        self.busStops = busStops
        self.LineId = lineId
        self.buses = buses

    @staticmethod
    def GetLineFromServer(lineId:str):
        if len(lineId) == 0:
            return None
        
        r = requests.get(baseUrl + "TransportLines/" + lineId)

        if r.status_code == 200:
            l = Line.LineWebSiteToLocalLine(r.json())
            Line.allLinesCache[lineId] = l
            return l
        else:
            print("Nisam uspeo da GETujem liniju: HTTP " + str(r.status_code))
            return None
    
    @staticmethod
    def LineWebSiteToLocalLine(data):
        #print('Vehicle iz LineWebSiteToLocalLine',data['Vehicles'])
        l = Line(
            data['TransportLineID'], 
            Line.ConvertBusLinePathDBToPath(data['LinePoints'], data['TransportLineID']), 
            Station.Station.StationsOnLinesToStation(data['Stations']),
            Vehicle.Vehicle.FromWebSiteToList(data['Vehicles'])
        )
        return l

    @staticmethod
    def ConvertBusLinePathDBToPath(busLinePathDB, LineId):
        path = [HaversineFormula.HaversineFormula.PointToRad((t['X'], t['Y'])) for t in busLinePathDB]
        return path

    @staticmethod
    def GetAllLinesFromServer():
        print("Ucitavanje linija sa servera")
        r = requests.get(baseUrl + 'TransportLines')

        if r.status_code == 200:
            listaLinija = [Line.LineWebSiteToLocalLine(linija) for linija in r.json()]
            dictLinija = {}
            for l in listaLinija:
                dictLinija[l.LineId] = l
            Line.allLinesCache = dictLinija
            return dictLinija
        else:
            print("Nisam uspeo da GETujem sve linije: HTTP " + str(r.status_code))
            return None
    
    @staticmethod
    def GetAllLinesWithBusesFromServer():
        print("Ucitavanje linija koje imaju bar jedan autobus povezan sa njima")
        r = requests.get(baseUrl + 'TransportLines/WithBuses')

        if r.status_code == 200:
            listaLinija = [Line.LineWebSiteToLocalLine(linija) for linija in r.json()]
            dictLinija = {}
            for l in listaLinija:
                dictLinija[l.LineId] = l
            Line.allLinesCache = dictLinija
            return dictLinija
        else:
            print("Nisam uspeo da GETujem sve linije: HTTP " + str(r.status_code))
            return None
    @staticmethod
    def GetLine(lineId):
        if lineId in Line.allLinesCache:
            return Line.allLinesCache[lineId]
        else:
            return Line.GetLineFromServer(lineId)