# 作業記録アプリ(Unity)

<img src="https://user-images.githubusercontent.com/58561188/200147419-3ac83943-3f9f-43ab-bca2-669b3c83d0b5.png" width="150"></li>
<img src="https://user-images.githubusercontent.com/58561188/200147422-2c7bd552-b3b5-4b7c-a1af-4b1c08a38cd8.png" width="150"></li>

## 使用法

作業記録アプリです。トータルの作業時間が記録されます。
以下のような用途で使うことを想定しています。

- 特定のタスクにかけた時間を記録したい(俺はまぁUnity1000時間、React500時間ぐらいだね～、など)
- 集中している時間を記録したい(タスク開始して10分しか経ってないのにYoutube見ようとしてるやんけ！など)

実行可能ファイルは含まれていません。

使用する際には2021.3.3f1以降のUnityで起動し、ビルドしてください。

## 目的

もう一つの目的としてUnityでデスクトップアプリを作った際の負荷について知りたい、という目的もありました。

解像度は350x600のウィンドウで、純粋なUI要素のみを使い、その他カメラなどの最適化は行わずデフォルトのままです。

測定にはタスクマネージャーを使用しました。測定項目はCPU、メモリです。

GPUに関しては少しでも使用されると高い値を出すので使用しません。

普段使用するアプリとしてChromeを比較対象とします。

![image](https://user-images.githubusercontent.com/58561188/200147989-7dd4e262-6b23-4f50-b8f9-9c84dcf1f74c.png)

結果を見るとメモリ効率は良いものの、CPU使用率はやや高い印象があります。

ただしCPU使用率はChromeで動画などの動的コンテンツを見た際にも同程度～5%程度まで上がるため、おおよそ同程度だと考えることができます。

結果としてChromeと比較した場合にメモリ効率が良いことはUnityでアプリを作るメリットと言えると思います。

Made with Unityは出ます。許せません。

# Task Time Recoder(Unity)

## Usage

This is a work recording application. Total work time is recorded.
It is intended to be used for the following purposes.

- To record the time spent on a specific task (I've been working on Unity for 1000 hours, React for 500 hours, not that much, not that much, etc.)
- To record the time spent concentrating (I started a task only 10 minutes ago and I'm trying to watch Youtube! OMG! etc.)

executable file is NOT included. (because of GitLFS or something idk)

you need to build to use this app by opening this repository in Unity(>= 2021.3.3f1)

## Purpose

Another objective was to know about the load when creating a desktop application in Unity.

The resolution was a 350x600 window, using only pure UI elements and no other optimizations such as cameras, etc. The default resolution was used.

Task Manager was used for the measurement. The measurement items were CPU and memory.

We did not use the GPU, as it shows huge value once it is used.

Chrome is used as a comparison target as it is an application that is usually used.

When I look at the result, although memory efficiency is good, I have an impression that CPU usage is a little high.

However, since the CPU usage rate rises to about the same level to about 5% when viewing dynamic contents such as videos in Chrome, it can be considered to be about the same level.

As a result, I think the better memory efficiency when compared to Chrome can be said to be an advantage of making apps with Unity.

Made with Unity is out. I can't forgive.
