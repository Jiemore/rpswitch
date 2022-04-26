import sys
import os
import RPi.GPIO as GPIO
import time

# 关机
def shotdown():
    os.system("shutdown now -h")

# 设备重启
def reboot():
    os.system("reboot")
    # sys.exit(0)

# 控制开关
def gpioSwitch():
    # 限制1分钟只能触发一次
    # 记录GPIO开关触发时间
    global TriggerTime
    
    if time.time() - TriggerTime > 60:
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(18,GPIO.OUT)
        GPIO.output(18,GPIO.HIGH)
        time.sleep(1)
        GPIO.output(18,GPIO.LOW)
        GPIO.cleanup()
        TriggerTime = time.time()
        print(TriggerTime)

TriggerTime = time.time() - 60



