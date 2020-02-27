import requests
import math

baseUrl = "http://localhost:52295/api/"

class Station():
    stationData = {}
    X = 1
    Y = 1
    naziv = ""

    def __init__(self, naziv,X,Y):
        self.X = X
        self.Y = Y
        self.naziv = naziv

    def GetPosition(self):
        return (self.X,self.Y)

    @staticmethod
    def StationsOnLinesToStation(StationsOnTransportLines):
        return [Station.GetStationsFromServer(stationsOnTransportLine['StationID']) for stationsOnTransportLine in StationsOnTransportLines]

    @staticmethod
    def GetStationsFromServer(stationId):
        r = requests.get(baseUrl + "Stations/" + stationId)

        if r.status_code == 200:
            data = r.json()
            Station.stationData[stationId] = Station(data['Name'], math.radians(data['X']), math.radians(data['Y']))
            return Station.stationData[stationId]
        else:
            print("Nisam uspeo da GET Vehicle: HTTP " + str(r.status_code))
            return None
    
    #verovatno isto nece trebati
    @staticmethod
    def Get(stationId):
        if stationId in Station.stationData:
            return Station.stationData[stationId]
        else:
            return Station.GetStationsFromServer(stationId)