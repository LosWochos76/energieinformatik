from PyCIM import cimread

d = cimread('220-kV-Netz-7Knoten-komplett_Rootnet_Area 1_EQ_V1.xml')
print(type(d))

for key in d.keys():
    print(key, " - ", d[key].UUID)
    d[key].UUID = "5"