#
import csv
import json
import sys

path = r'C:\Users\Mladjan\Desktop\data.json'
outputPath = r'C:\Users\Mladjan\Desktop\Web2\3AStopsdata.txt'

inputFile = open(path,encoding="utf8")
outputFile = open(outputPath, 'w',encoding="utf8")

data = json.load(inputFile)
inputFile.close()


for i in data['3A']:
    outputFile.write('{},{},{}\n'.format(i['longitude'],i['latitude'],i['station']))
outputFile.close()

outputPath = r'C:\Users\Mladjan\Desktop\Web2\3BStopsdata.txt' 
outputFile = open(outputPath, 'w',encoding="utf8")
for i in data['3B']:
    outputFile.write('{},{},{}\n'.format(i['longitude'],i['latitude'],i['station']))
outputFile.close()

outputPath = r'C:\Users\Mladjan\Desktop\Web2\64AStopsdata.txt'
outputFile = open(outputPath, 'w',encoding="utf8")
for i in data['64A']:
    outputFile.write('{},{},{}\n'.format(i['longitude'],i['latitude'],i['station']))
outputFile.close()

outputPath = r'C:\Users\Mladjan\Desktop\Web2\64BStopsdata.txt'
outputFile = open(outputPath, 'w',encoding="utf8")
for i in data['64B']:
    outputFile.write('{},{},{}\n'.format(i['longitude'],i['latitude'],i['station']))
outputFile.close()
