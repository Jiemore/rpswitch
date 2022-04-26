import logging
import ifcfg
import json
import time
import psutil
import datetime
import uptime
import uuid
from os import getuid
from signalrcore.hub_connection_builder import HubConnectionBuilder
import localMethod


# --- config start ---
# logging.basicConfig(datefmt = '%a, %d %b %Y %H:%M:%S',
#                     filename = 'rp.log',
#                     filemode = 'w',  # 每次会重新写日志，覆盖之前的日志
#                     format = '%(asctime)s - %(pathname)s[line:%(lineno)d] - %(levelname)s: %(message)s'
#                     )
# server_url = "wss://localhost:5001/chatHub"
server_url = "wss://example.com:5001/chatHub"
username = "raspi-k"
tag = "raspi"
deviceid = str(uuid.uuid4())
handler = logging.StreamHandler()
handler.setLevel(logging.DEBUG)

# --- config end ---

# 系统资源信息


def GetBaseInfo():
    return {
        "tag": tag,
        "time": time.strftime("%Y-%m-%d %H:%M:%S"),
        "id": deviceid,
        "sys": {
            "ip": getInterface(),
            "memory": psutil.virtual_memory().percent,
            "cpu": psutil.cpu_percent(None),
            # str(datetime.timedelta(seconds=uptime.uptime())).split(":"),
            "uptime": uptime.uptime(),
        }
    }

# 网络接口信息


def getInterface():
    device = {}
    for name, interface in ifcfg.interfaces().items():
        # do something with interface
        # device["interface"][interface["device"]] = interface["inet"]
        if interface["inet"] is None:
            continue

        device[interface["device"]] = interface["inet"]
    return device
    # return json.dumps(device)

# 电源/开关机


def executeMethod(msg):
    method = [localMethod.shotdown, localMethod.reboot, localMethod.gpioSwitch]
    if deviceid in msg:
        logging.info("excute:" + method[msg[1]].__name__)
        method[msg[1]]()


if __name__ == "__main__":
    # st = json.dumps(GetBaseInfo(), indent=2)
    # print(st)
    # sys.exit(0)
    while True:
        try:
            # print(msg)
            # 连接到中心
            hub_connection = (
                HubConnectionBuilder()
                .with_url(server_url, options={"verify_ssl": False})
                .configure_logging(logging.DEBUG, socket_trace=True, handler=handler)
                .with_automatic_reconnect(
                    {
                        "type": "interval",
                        "keep_alive_interval": 10,
                        "intervals": [1, 3, 5, 6, 7, 87, 3],
                    }
                )
                .build()
            )

            hub_connection.on_open(
                lambda: print(
                    "connection opened and handshake received ready to send messages"
                )
            )
            hub_connection.on_close(lambda: print("connection closed"))

            hub_connection.on("Execute", executeMethod)
            hub_connection.start()

            while True:
                msg = json.dumps(GetBaseInfo())  # , indent=4)
                hub_connection.send("SendMessage", [username, msg])
                time.sleep(2)

        # 连接到中心失败
        except Exception as e:
            logging.error("No connection...\n" + str(e) + str(datetime.datetime.now()))
            print("LogDetail:ERROR-" + str(e) + str(datetime.datetime.now()))

        finally:
            hub_connection.stop()
            logging.debug("LogDetail:Finally-" + str(datetime.datetime.now()))
            time.sleep(5)
