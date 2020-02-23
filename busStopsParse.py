import json

#skrejpovani csv file location
path = r'C:\Users\Mladjan\Desktop\busStops.csv'
filteredData = [] 
splittedData = []

jsonData  = {'3A': [], '3B': [], '64A': [], '64B': []}
with open(path,'r',encoding="utf8") as f: 
    data = f.readlines()
    for i in range(0,len(data)):
        if( '[3A]' in data[i] or '[3B]' in data[i] or '[64A]' in data[i] or '[64B]' in data[i]):
            filteredData.append(data[i])
    
    for i in range(0,len(filteredData)):
        splittedData = filteredData[i].split('|')
        if '[3A]' in splittedData[0]:
            if not jsonData['3A'].__contains__({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]}):
                jsonData['3A'].append({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]})
        if '[3B]' in splittedData[0]:
            if not jsonData['3B'].__contains__({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]}):
                jsonData['3B'].append({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]})
        if '[64A]' in splittedData[0]:
            if not jsonData['64A'].__contains__({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]}):
                jsonData['64A'].append({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]})
        if '[64B]' in splittedData[0]:
            if not jsonData['64B'].__contains__({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]}):
                jsonData['64B'].append({'longitude': float(splittedData[1]),'latitude': float(splittedData[2]),'station': splittedData[3]})

            
with open('data.json','w',encoding="utf8") as outline:
    json.dump(jsonData,outline,ensure_ascii=False,indent=2)
