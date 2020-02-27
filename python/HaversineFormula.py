import math

#precnik zemlje(m)
R = 6356000



class HaversineFormula():
    #svi proracuni su u radijanima!

    #region Konverzije u radijane
    @staticmethod
    def PointToRad(p):
        return (math.radians(p[0]), math.radians(p[1]))

    @staticmethod
    def PathToRad(path):
        return [HaversineFormula.PointToRad(p) for p in path]
    #endregion

    #region Haversine formula
    #umesto u sin formatu zapisana u cos formatu
    '''
    formule za koriscenje
    a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2)
    c = 2 ⋅ atan2( √a, √(1−a) )
    d = R ⋅ c
    where	φ is latitude, λ is longitude, R is earth’s radius
    note that angles need to be in radians to pass to trig functions!
    '''
    @staticmethod
    def Distance(p1,p2):
        a = 0.5 - math.cos((p2[0] - p1[0]))/2 + math.cos(p1[0]) * math.cos(p2[0]) * (1 - math.cos((p2[1] - p1[1]))) / 2
        return 2 * R * math.asin(math.sqrt(a))

    #Bearing: The horizontal angle at a given point, measured clockwise from magnetic north or true north to a second point.
    '''
    Let ‘R’ be the radius of Earth,
        ‘L’ be the longitude,
        ‘θ’ be latitude,
        ‘β‘ be Bearing.
        Bearing from point A to B, can be calculated as,

        β = atan2(X,Y),

        where, X and Y are two quantities and can be calculated as:

        X = cos θb * sin ∆L

        Y = cos θa * sin θb – sin θa * cos θb * cos ∆L
    '''
    @staticmethod
    def CalculateBearing(p1Rad,p2Rad):
        A = math.cos(p2Rad[0]) * math.sin(p2Rad[1] - p1Rad[1])
        B = (math.cos(p1Rad[0]) * math.sin(p2Rad[0])) - (math.sin(p1Rad[0]) * math.cos(p2Rad[0]) * math.cos(p2Rad[1] - p1Rad[1]))
        bearing = math.atan2(A,B)
        return bearing

    '''
    Let first point latitude be la1,
        longitude as lo1,
        d be distance,
        R as radius of Earth,
        Ad be the angular distance i.e d/R and
        θ be the bearing,
        Here is the formula to find the second point, when first point, bearing and distance is known:

        latitude of second point = la2 =  asin(sin la1 * cos Ad  + cos la1 * sin Ad * cos θ), and
        longitude  of second point = lo2 = lo1 + atan2(sin θ * sin Ad * cos la1 , cos Ad – sin la1 * sin la2)
    '''
    @staticmethod
    def MovePointAlongAngle(pRad, bearingRad, distance):
        Ad = distance / R
        la2 =  math.asin(math.sin(pRad[0]) * math.cos(Ad) + math.cos(pRad[0]) * math.sin(Ad) * math.cos(bearingRad))
        lo2 = pRad[1] + math.atan2(math.sin(bearingRad) * math.sin(Ad) * math.cos(pRad[0]), math.cos(Ad) - math.sin(pRad[0]) * math.sin(la2))
        #vrati novu tacku
        return (la2,lo2)
    #endregion

    @staticmethod
    def Move(pStart,pEnd, stepInMeters, precisionInMeters):
        if precisionInMeters < stepInMeters:
            return None

        while HaversineFormula.Distance(pStart, pEnd) > precisionInMeters:
            bearing = HaversineFormula.CalculateBearing(pStart, pEnd)
            pStart = HaversineFormula.MovePointAlongAngle(pStart, bearing, stepInMeters)
            yield pStart

    @staticmethod
    def MoveAlongRoute(path, stepInMeters):
        precisionInMeters = stepInMeters + 0.01

        pStart = path[0]
        for i in range(len(path) - 1):
            pEnd = path[i + 1]

            if HaversineFormula.Distance(pStart, pEnd) < stepInMeters:
                continue
            
            for pStart in HaversineFormula.Move(pStart, pEnd, stepInMeters, precisionInMeters):
                yield pStart
