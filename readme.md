# 开发日程

9.7 关卡搭建 主菜单 bgm 优化移动跳跃
9.6 实现精力宝石 踩上去会消失的石头 优化蓄力跳跃
9.5 改善射线检测，使用多条射线 爬墙到顶后自动往前走 摄像机SmoothDamp
9.4 蓄力跳跃 通过颜色显示能否冲刺 基本摄像机跟随 命名规范
9.3 重写状态机 冲刺平滑 加入注释
9.2 状态机 加入射线检测 基本爬墙
9.1 人物基本移动、跳跃



# 解决方案

## 人物移动

- GetAxis 惯性过大 GetAxisRaw 没有惯性
  AnimationCurve  两条曲线分别记录加速和减速过程

- 下落速度过快 
  限制最大下落速度

- 其他优化

  加入材质

## 跳跃

- AddForce、Translate、velocity 无法实现想要效果
  使用AnimationCurve
- 上升时速度不对
- 取消重力，全部由速度曲线控制

### 蓄力跳跃

- 使用计时器记录蓄力时间导致蓄力时间完成后才能跳跃，跳跃有延迟
  改为每帧加速度，计时器记录时间
- 跳跃不灵
  射线检测地面 在离地面较短距离就能跳跃

### 冲刺跳跃

- 普通移动速度乘以倍率导致没有惯性，也没有减速过程
  跳跃时移动用另一条曲线记录速度

## 爬墙

- 触墙时向墙移动不会落下  
  触墙时取消普通移动
- 下落速度过快 
  触墙时按相应方向键调整重力
- 使用oncollision检测墙体 爬上后依然会被检测到 
  射线检测 
- 中央发射左右射线 高出一半检测不到 
  底部发射左右射线
- 爬到顶端后没有自动走上地面 离开墙体 在
  玩家朝向方向有位移
- 中途跳跃有抖动 
  加入按住方向键上才有位移

## 冲刺

- Update()中使用Translate等 会造成瞬移
  采用状态机，每个状态继承接口中enter,update,fixedupdate,finish方法 使用for进行循环 
- 每次循环方向可能改变 
  在一开始确定方向再开始循环
- 冲刺中有其他位移  
  冲刺前重力和速度置零

## 检测地面

- OnCollision() 只有在接触后才能跳跃并且不能判断接触方向
  射线检测，由物体中央向下发射射线   
- 当物体在边缘时检测不到
  多条射线检测由中央向左下右下发射两条射线进行检测

## 场景

### 摄像机

- 普通跟随角色导致画面抖动太厉害 
  使用Smoothdamp 
- 依然抖动 
  多数场景改为固定摄像机，在切换场景时用Smoothdamp

### 玩家状态

- 通过最后的水平速度判断朝向导致墙体冲刺时反弹会时方向变反 
  通过最后一次方向按键判断朝向
- 死亡后复活有惯性  
  死亡后setactive false 隔一段时间复活

### 道具

- 脚本写在触发器中会导致检测不到istrigger
  取消触发器，使用oncollision

## 画面卡顿，按键不灵

- 使用fixedupdate