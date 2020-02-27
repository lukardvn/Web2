import TransportLine
import Vehicle
import HaversineFormula
import Station
import json
import itertools
import time

baseUrl = "http://localhost:52295/api/"

#kod ovih linija je sekvenca tacaka pogresno napisana
sortReverse = []

def LoadConfig():
    try:
        with open('config.json', 'rb') as f:
            options = json.loads(f.read().decode('utf-8'))
            return options
    except Exception as e:
        print(str(e))
        print("Nema config.json fajla")
        return None

def Main():
    simulationSpeed = 1
    sendDataToServerInterval = 1

    updateBusPositionInterval = 0.1

    getLines = []
    config = LoadConfig()
    if not config is None:
        print("config.json je ucitan")
        simulationSpeed = config['simulationSpeed']
        sendDataToServerInterval = config['updateServerInterval']
        global baseUrl
        baseUrl = config['baseUrl']
        global sortReverse
        sortReverse = config['reverseLines']
        getLines = config['simulateLines']
        Vehicle.Vehicle.minTimeOnBusStop = config['minBusStopTime']
        Vehicle.Vehicle.maxTimeOnBusStop = config['maxBusStopTime']

    Vehicle.Vehicle.updateBusPositionInterval = updateBusPositionInterval
    updateBusPositionInterval *= (1 / simulationSpeed)
    
    if len(getLines) > 0:
        print("Radim simulaciju za sledece linije: " + ",".join(getLines))
        for lineCode in getLines:
            TransportLine.Line.GetLine(lineCode)
    else:
        TransportLine.Line.GetAllLinesWithBusesFromServer()


    #postavljanje putanja po kojima e da idu autobusi
    for line in list(TransportLine.Line.allLinesCache.values()):
        for bus in line.buses:
            bus.SetPath(line.path)
    print("linije postavljene sada postavljanje autobusa na linije i pocetak simulacije")
    timer = 0

    #glavni loop simulacije
    while True:
        for bus in list(Vehicle.Vehicle.vehicleData.values()):
            p = next(bus.Move(), None)

            if p is None:
                print(bus.Id + " je stigao do kraja")
            else:
                pass

        time.sleep(updateBusPositionInterval)
        timer += updateBusPositionInterval

        if timer >= sendDataToServerInterval:
            Vehicle.Vehicle.SendBusesToServer(Vehicle.Vehicle.vehicleData.values())
            timer = 0

if __name__ == "__main__":
    Main()