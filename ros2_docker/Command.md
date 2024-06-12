# Command



#### 在以下位置打开 CMD

```
D:\Projects\Unity\Robotics-Nav2-SLAM-Example\Robotics-Nav2-SLAM-Example\ros2_docker
```

#### 运行以下命令

```
docker run -it --rm -p 6080:80 -p 10000:10000 --shm-size=1024m unity-robotics:nav2-slam-example
```

#### 若出现端口被占用

```
netstat -aon|findstr "10000"	# 10000为端口号
taskkill /T /F /PID 9088	# 9088 为进程号
```

#### 可能会导致 Docker Desktop 重启的现象，直接从 Docker Desktop 启动或许是个更好的办法



#### 在浏览器 novnc2 中运行以下命令，实现 unity 内信息可视化

```
ros2 launch unity_slam_example unity_viz_example.py
```

#### 之后运行 unity 内项目，应该就可以正确连接了



#### 若出现以下错误

```
[controller_server-1] [INFO] [1718182603.753304920] [local_costmap.local_costmap]: Timed out waiting for transform from base_link to odom to become available, tf error: Invalid frame ID "odom" passed to canTransform argument target_frame - frame does not exist
...
[controller_server-1] [INFO] [1718182605.253381757] [local_costmap.local_costmap]: Timed out waiting for transform from base_link to odom to become available, tf error: Invalid frame ID "base_link" passed to canTransform argument source_frame - frame does not exist
```

### 尝试在novnc2 中运行以下命令

```
ros2 launch nav2_bringup tb3_simulation_launch.py slam:=True
```



#### 接下来通过 CMD 控制台向 Docker 输入坐标命令，控制小车前往指定位置（管理员模式）

```
docker exec -it unruffled_sanderson /bin/bash
# unruffled_sanderson 是容器的名字，请在 Docker Desktop 中查看

ros2 topic pub -1 /goal_pose geometry_msgs/PoseStamped "{header: {stamp: {sec: 0}, frame_id: 'map'}, pose: {position: {x: 0.0, y: -4.0, z: 0.0}, orientation: {w: 1.0}}}"
```

#### 若在 Docker 容器中执行 Ros2 命令时出现 `bash: ros2: command not found` 错误，执行以下命令

```
source /opt/ros/<ros_distro>/setup.bash
# 其中 ros_distro 可以通过以下命令来获取，这里应该是 galactic
echo $ROS_DISTRO
```

