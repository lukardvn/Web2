import random
import time
import math
import requests
import HaversineFormula
import TransportLine

baseUrl = "http://localhost:52295/api/"

class Vehicle():
    vehicleData = {}
    X = 1 #u radijanima
    Y = 1 #u radijanima
    Id = ""
    path = None
    iterator = None
    OnLine = ""
    firstStart = True
    speed = 1 #km/h
    updateBusPositionInterval = 0.5 #sec
    timer = 0
    lastBusStop = None
    minTimeOnBusStop = 5
    maxTimeOnBusStop = 13
    waitTimeOnNextBusStop = -1

    def __init__(self, Id,X,Y,OnLine):
        self.X = X
        self.Y = Y
        self.Id = Id
        self.OnLine = OnLine
        self.firstStart = True
        Vehicle.vehicleData[Id] = self

    def SetPosition(self,point):
        self.X = point[0]
        self.Y = point[1]
    
    def SetPath(self,path):
        if len(path) == 0:
            self.path = None
            return

        self.path = path
        #postavim pocetnu poziciju autobusa (na pocetak linije)
        self.X = self.path[0][0]
        self.Y = self.path[0][1]

    def ChangeDirection(self):
        staraLinija = self.OnLine
        smer = self.OnLine[-1]
        if smer == "A":
            smer = "B"
        else:
            smer = "A"
        
        self.OnLine = self.OnLine[:-1] + smer
        print(self.Id + " dosao sam do kraja linije "+staraLinija+", sad se vracam nazad linijom " + self.OnLine)

    def Move(self):
        if self.path is None:
            yield None

        if self.iterator is None:
            pocetnaTacka = 0
            if self.firstStart:
                random.seed(str(time.time()) + self.Id + self.Id) 
                pocetnaTacka = random.randint(0, len(self.path) - 4)
                self.firstStart = False
            
            stepLength = (Vehicle.speed * 1000 / 3600) / Vehicle.updateBusPositionInterval #u metrima koliko ce autobus preci tokom jednog intervala updateovanja
            self.iterator = HaversineFormula.HaversineFormula.MoveAlongRoute(self.path[pocetnaTacka:], stepLength)
        
        #ovo nece trebati malo sam se precenio
        #region NeceTrebati
        busStop = self.CheckForBusStop()

        if not busStop is None:
            if self.waitTimeOnNextBusStop == -1:
                self.waitTimeOnNextBusStop = random.randint(Vehicle.minTimeOnBusStop, Vehicle.maxTimeOnBusStop)

            while self.timer < self.waitTimeOnNextBusStop:
                self.timer += self.updateBusPositionInterval
                self.SetPosition(busStop.GetPosition())
                yield self.GetPosition()
            
            self.waitTimeOnNextBusStop = -1
            self.lastBusStop = busStop
            self.timer = 0
            newPosition = self.GetPosition()
        else:
            newPosition = next(self.iterator, None)

        if newPosition is None:
            self.iterator = None
            self.ChangeDirection()
            self.path = TransportLine.Line.GetLine(self.OnLine).path
            yield self.GetPosition()
        else:
            self.SetPosition(newPosition)
            yield self.GetPosition()
        #endregion

    #region niOvo ali zbog indentiona mora da se ovako uradi haha
    def CheckForBusStop(self):
        #simulacija zaustavljanja autobusa na stanici
        for stanica in TransportLine.Line.allLinesCache[self.OnLine].busStops:
            if HaversineFormula.HaversineFormula.Distance(self.GetPosition(), stanica.GetPosition()) <= 16:
                if stanica == self.lastBusStop:
                    return None
                return stanica
        
        return None
    #endregion

    def GetPosition(self):
        return (self.X, self.Y)
    
    def __str__(self):
        return self.Id + "," + str(math.degrees(self.X)) + "," + str(math.degrees(self.Y)) + "," + self.OnLine

    @staticmethod
    def FromWebSiteToList(listOfWBBuses):
        return [Vehicle(vehicle['VehicleID'], math.radians(vehicle['X']), math.radians(vehicle['Y']), vehicle['TransportLineID']) for vehicle in listOfWBBuses]

    @staticmethod
    def SendBusesToServer(listOfBuses):
        data = "|".join([str(Vehicle) for Vehicle in listOfBuses])
        r = requests.post(baseUrl + 'vehicles/updateVehiclePosition', data=data)
        if r.status_code == 200:
            print("Nove pozicije poslate serveru (buseva poslato:" + str(len(listOfBuses)) + ")")
        else:
            print("Greska prilikom updateovanja pozicija buseva: HTTP " + str(r.status_code))
    
    @staticmethod
    def GetBus(busId):
        if busId in Vehicle.vehicleData:
            return Vehicle.vehicleData[busId]
        return None
