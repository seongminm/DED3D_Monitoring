# DED_Monitoring
이 리포지스토리는 DED 방식 3D 프린터에 부착되는 센서 모너티렁을 위한 리포지스토리입니다.

## Contents 
- [소개](https://github.com/37inm/DED_Monitoring#%EC%86%8C%EA%B0%9C)
- [개발환경](https://github.com/37inm/DED_Monitoring#%EA%B0%9C%EB%B0%9C%ED%99%98%EA%B2%BD)

## 소개
> DED 3D 프린터에 부착하여 최적 공정 조건을 찾기 위해 제작되었습니다.
<img src="https://github.com/37inm/DED_Monitoring/assets/131761210/407c2736-e759-4387-a3c7-2b55fe65edf8" width="600"/>
<img src="https://github.com/37inm/DED_Monitoring/assets/131761210/2248f48d-3161-4f3d-afc2-72809035da6b" width="600"/>

이 프로그램은 직접 개발한 센서 모듈과 UART, UDP 통신을 통해 실시간으로 데이터를 확인 할 수 있는 유틸리티 프로그램입니다.

<img src="https://github.com/37inm/DED_Monitoring/assets/131761210/d08fafb6-0cc5-4f39-a8fc-53911a847159" width="800"/>

csv파일 형식, MySQL 데이터베이스에 실시간 평균값, 표준편차 데이터를 저장할 수 있는 기능을 제공합니다.


<img src="https://github.com/37inm/DED_Monitoring/assets/131761210/0bcd96d0-4ce2-43fd-8df0-553c9602cea1" width="600"/>

센서 모듈의 상태를 확인해야 할 경우 Udp 탭을 통해 모듈의 출력값을 확인하는 기능을 제공합니다.

## 개발환경
:heavy_check_mark: WPF [.NET Framework 4.8](https://dotnet.microsoft.com/ko-kr/download/dotnet-framework/net48)

:heavy_check_mark: Visual Studio 2019

<img src="https://github.com/37inm/GrblController/assets/131761210/673f9ef5-07f9-48ee-aaf2-7e659e2c8af7" width="400"/>