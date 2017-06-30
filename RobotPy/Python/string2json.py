import json

def drawRectangle():
    print("yay")

message = '{"func":"drawRectangle", "args":["white_pixel","350","362","4","4","White"]}'
d = json.loads(message)
print(int(d["args"][1]) + int(d["args"][1]) )

k = d["func"]
eval(k)()