import os
from openai import AzureOpenAI

os.environ["http_proxy"] = "http://localhost:7890"
os.environ["https_proxy"] = "http://localhost:7890"

client = AzureOpenAI(
  azure_endpoint = os.getenv("AZURE_OPENAI_ENDPOINT"), 
  api_key=os.getenv("AZURE_OPENAI_API_KEY"),  
  api_version="2023-05-15"
)

# 定义提示信息和用户问题
prompt_message = """
你是一个智能家居领域的专家。你的任务是将智能家居领域的复杂用户需求分解为一个资源API序列。
资源API序列应当是一个包含多个资源API，并符合以下json格式：
{
    "task_sequence": []
}
其中每个资源API都应符合以下格式：
{
	"task_name": "",
	"api_call": "",
	"parameters": { },
	"order": ""
}
"task_name"为子任务名称，
"api_call"为接口调用名称，
"parameters"为接口调用所需的参数，
"order"为接口调用的先后顺序。

以下是可用的资源API表单：
子任务分类,智能家居中可能的应用场景,接口调用名称,参数
人脸识别模型,智能门锁系统：通过位于门上的摄像头，获取其人脸图像。和已存储的家庭成员的面部特征进行对比来进行身份验证。,FaceRecognition(),无
温度检测算法,"室内温度检测：检测室内各房间的温度
室外温度检测：调用天气预报服务检测室外温度",DetectTemp(),无
湿度检测算法,"室内湿度检测：检测室内各房间的湿度
室外湿度检测：调用天气预报服务检测室外湿度",DetectHumdity(),无
亮度检测算法,"室内亮度检测：检测室内各房间的亮度
室外亮度检测：检测阳台或屋顶亮度",DetectBrightness(),无
灯光控制算法,室内各灯泡状态控制：控制室内各灯泡的开关状态,LightCtrl(),"string place
可能的place包括LivingRoom、BedRoom1、BedRoom2、DiningRoom、Kitchen、Toliet"
窗帘控制算法,室内各窗帘状态控制：控制室内各窗帘的开合状态,CurtainCtrl(),"string place
可能的place包括LivingRoom"
窗户控制算法,室内各窗户状态控制：控制室内各窗户的开合状态,WindowCtrl(),无
门控制算法,室内各门开合状态控制：控制室内各门的开合状态,DoorCtrl(),"string place
可能的place包括LivingRoom、BedRoom1、BedRoom2、DiningRoom、Kitchen、Toliet"
空调控制算法,室内各空调状态控制：控制室内各空调的开关状态,AirConditionerCtrl(),string temperature
电扇控制算法,室内各电扇状态控制：控制室内各电扇的开关状态,FanCtrl(),无
加湿器控制算法,室内各加湿器状态控制：控制室内各加湿器的开关状态,HumidifierCtrl(),无
除湿器控制算法,室内各除湿器状态控制：控制室内各除湿器的开关状态,DehumidifierCtrl(),无
扫地机器人控制算法,扫地机器人状态控制：控制室内扫地机器人的开关状态,SweeperCtrl(),无

生成的内容应不要包含任何中文、辅助信息、解释信息和说明。仅保留json格式的资源API序列！
生成的内容应不要包含任何中文、辅助信息、解释信息和说明。仅保留json格式的资源API序列！
生成的内容应不要包含任何中文、辅助信息、解释信息和说明。仅保留json格式的资源API序列！
"""

# 用户提出具体问题
user_question = """
我十分钟后回家，帮我把屋子变的暖和一些。
"""

# 发送请求
response = client.chat.completions.create(
    model="ArkidianChatGPT35Turbo", # 请替换为实际的模型部署名称
    messages=[
        {"role": "system", "content": "You are an expert in smart home technology. Your task is to break down complex user requirements in the smart home field into a sequence of resource APIs. Here is a table of available resource APIs:"},
        {"role": "system", "content": prompt_message},
        {"role": "user", "content": user_question}
    ]
)

# 获取模型的回复
reply = response.choices[0].message.content

# 将结果写入到TXT文件
with open("smart_home_api_response.txt", "w", encoding="utf-8") as file:
    file.write(reply)

print("The response has been written to smart_home_api_response.txt")