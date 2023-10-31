# DED_Monitoring
이 리포지스토리는 DED 방식 3D 프린터에 부착되는 센서 모너티렁을 위한 리포지스토리입니다.

## Contents 
- 소개
- 개발환경

## 소개
> DED 3D 프린터에 부착하여 최적 공정 조건을 찾기 위해 제작되었습니다.
</br>
<img src="https://github.com/37inm/DED3D_Monitoring/assets/131761210/e50f34ee-d230-458a-9025-a12ff44540ab" width="600" height="600"/>

> 이 프로그램은 직접 개발한 센서 모듈과 UART, UDP 통신을 통해 실시간으로 데이터를 확인 할 수 있습니다.
</br>
<img src="https://github.com/37inm/DED3D_Monitoring/assets/131761210/e2b4d4b5-2730-4668-b320-6a26a811fb84" width="600" height="600"/>
<img src="https://github.com/37inm/DED3D_Monitoring/assets/131761210/6cc42dc4-d503-47f7-898d-2aa54a7149af" width="600"/>

> csv파일 형식, MySQL 데이터베이스에 실시간 평균값, 표준편차 데이터를 저장할 수 있는 기능을 제공합니다.
</br>
<img src="https://github.com/37inm/DED3D_Monitoring/assets/131761210/7e124e11-82f0-4d1e-8e9c-55663714760a" width="600" height="600"/>

> Udp 탭을 통해 모듈의 출력값을 직접 확인하는 기능을 제공합니다.
</br>

## 개발환경
:heavy_check_mark: WPF [.NET Framework 4.8](https://dotnet.microsoft.com/ko-kr/download/dotnet-framework/net48)

:heavy_check_mark: Visual Studio 2019

<img src="https://github.com/37inm/GrblController/assets/131761210/673f9ef5-07f9-48ee-aaf2-7e659e2c8af7" width="400"/>
