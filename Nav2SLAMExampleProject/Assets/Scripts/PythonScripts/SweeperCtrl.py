import subprocess
import sys

def execute_docker_command(x, y, z):
    try:
        # 进入 Docker 容器
        container_name = "unruffled_sanderson"
        docker_exec_cmd = f"docker exec -it {container_name} /bin/bash"
        
        # 检查 ros2 命令是否可用，并设置环境变量
        setup_ros_cmd = """
        if ! command -v ros2 &> /dev/null
        then
            source /opt/ros/$(echo $ROS_DISTRO)/setup.bash
        fi
        """
        
        # 发布 ROS2 目标位置信息
        ros2_command = f"""
        ros2 topic pub -1 /goal_pose geometry_msgs/PoseStamped "{{
            header: {{
                stamp: {{sec: 0}}, 
                frame_id: 'map'
            }}, 
            pose: {{
                position: {{x: {x}, y: {y}, z: {z}}}, 
                orientation: {{w: 1.0}}
            }}
        }}"
        """

        full_command = f"{docker_exec_cmd} -c '{setup_ros_cmd} && {ros2_command}'"
        subprocess.run(full_command, shell=True, check=True)
        print("Command executed successfully.")

    except subprocess.CalledProcessError as e:
        print(f"Error occurred: {e}")

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("Usage: python script.py <x> <y> <z>")
        sys.exit(1)
    
    x = float(sys.argv[1])
    y = float(sys.argv[2])
    z = float(sys.argv[3])

    execute_docker_command(x, y, z)
