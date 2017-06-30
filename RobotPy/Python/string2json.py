import json

message = '{"func":"drawRectangle", "args":["white_pixel","350","362","4","4","White"]}'
d = json.loads(message)
print(d["args"][0])
