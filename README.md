# 作業記録アプリ(Unity)

<img src="https://user-images.githubusercontent.com/58561188/200147419-3ac83943-3f9f-43ab-bca2-669b3c83d0b5.png" width="150"></li>
<img src="https://user-images.githubusercontent.com/58561188/200147422-2c7bd552-b3b5-4b7c-a1af-4b1c08a38cd8.png" width="150"></li>

## 使用法

作業記録アプリです。トータルの作業時間が記録されます。
以下のような用途で使うことを想定しています。

- 特定のタスクにかけた時間を記録したい(俺はまぁUnity1000時間、React500時間ぐらいだね～、など)
- 集中している時間を記録したい(タスク開始して10分しか経ってないのにYoutube見ようとしてるやんけ！など)

buildファイル内に実行可能ファイルが置いてあります。

## 目的

もう一つの目的としてUnityでデスクトップアプリを作った際の負荷について知りたい、という目的もありました。

解像度は350x600のウィンドウで、純粋なUI要素のみを使い、その他カメラなどの最適化は行わずデフォルトのままです。

結果としてはメモリ効率が非常に良く、CPUやGPU使用率はやや高い程度で、許容範囲内です(タスクマネージャーで測定)。
どの項目もChromeでYoutubeを見るときの1/10くらいの値に収まっていました。

これから言えることは、デスクトップアプリのためだけにTkinterをはじめとする別フレームワークを学習するよりは、
デザインのカスタマイズが簡単なUnityをそのまま使っても良いかもしれません。

Made with Unityは出ます。許せません。

# Task Time Recoder(Unity)

## Usage

This is a work recording application. Total work time is recorded.
It is intended to be used for the following purposes.

- To record the time spent on a specific task (I've been working on Unity for 1000 hours, React for 500 hours, not that much, not that much, etc.)
- To record the time spent concentrating (I started a task only 10 minutes ago and I'm trying to watch Youtube! OMG! etc.)

There is .exe in ClockExe directory.

## Purpose

Another purpose was to know the load when creating a desktop application in Unity.

The resolution was a 350x600 window, using only pure UI elements, and no other optimizations such as camera, etc.

The result is very good memory efficiency, CPU and GPU utilization are only slightly high and within acceptable limits (measured by Task Manager).
All items were within about 1/10 of the values when viewing Youtube in Chrome.

I can say from this that rather than learning Tkinter and other separate frameworks just for desktop apps
You may as well just use Unity as it is, which is easy to customize the design.

Made with Unity will be out. I never forgive this.
