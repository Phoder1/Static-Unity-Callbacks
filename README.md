# com.alontalmi.unitycallbacks
 A static Unity callbacks library inspired by the amazing post by TheBeardedPhantom:
 https://blog.beardphantom.com/post/190674647054/unity-2018-and-playerloop
 
 To use you can simply use:
 UnityCallbacks.{EventName} += {MethodName}
 
I also added a static and pausable coroutines handler, mostly for practice, but it sure can be usefull as well! 

But please notice a few diffrences from Unity's Coroutines:
1) It does not support Coroutine and yieldInstruction types! 
But it does support CustomYieldInstruction and AsyncOperation, so some conversion work might be required.
2) It's not linked to any gameobject (Duhh.. it's static), so stopping the coroutine when the gameobject is destroyed has to be called manually.
3) If you want the to wait to another coroutine inside another coroutine just Yield Return the IEnumerator! It's not required to call Startcoroutine.
4) All coroutines are pausable! If you will stop a coroutine, keep the refrence, and call start coroutine with that reference again later it will start from the yield instruction it was at last.
5) If you want explicit pause/resume functionallity, you can use the PausableYield class. (See the WaitForSecondsPausable class for refrence)
 
 Entire Unity callbacks tree (can also be seen inside the post, but was copied here for comfort, also notice this was created by TheBeardedPhantom and might not be updated to the latest version, post was made at Feb 5th, 2020):
 
    #Initialization
    - PlayerUpdateTime
    - AsyncUploadTimeSlicedUpdate
    - SynchronizeInputs
    - SynchronizeState
    - XREarlyUpdate
    #EarlyUpdate
    - PollPlayerConnection
    - ProfilerStartFrame
    - GpuTimestamp
    - UnityConnectClientUpdate
    - CloudWebServicesUpdate
    - UnityWebRequestUpdate
    - ExecuteMainThreadJobs
    - ProcessMouseInWindow
    - ClearIntermediateRenderers
    - ClearLines
    - PresentBeforeUpdate
    - ResetFrameStatsAfterPresent
    - UpdateAllUnityWebStreams
    - UpdateAsyncReadbackManager
    - UpdateTextureStreamingManager
    - UpdatePreloading
    - RendererNotifyInvisible
    - PlayerCleanupCachedData
    - UpdateMainGameViewRect
    - UpdateCanvasRectTransform
    - UpdateInputManager
    - ProcessRemoteInput
    - XRUpdate
    - TangoUpdate
    - ScriptRunDelayedStartupFrame
    - UpdateKinect
    - DeliverIosPlatformEvents
    - DispatchEventQueueEvents
    - DirectorSampleTime
    - PhysicsResetInterpolatedTransformPosition
    - NewInputBeginFrame
    - SpriteAtlasManagerUpdate
    - PerformanceAnalyticsUpdate
    #FixedUpdate
    - ClearLines
    - NewInputEndFixedUpdate
    - DirectorFixedSampleTime
    - AudioFixedUpdate
    - ScriptRunBehaviourFixedUpdate
    - DirectorFixedUpdate
    - LegacyFixedAnimationUpdate
    - XRFixedUpdate
    - PhysicsFixedUpdate
    - Physics2DFixedUpdate
    - DirectorFixedUpdatePostPhysics
    - ScriptRunDelayedFixedFrameRate
    - ScriptRunDelayedTasks
    - NewInputBeginFixedUpdate
    #PreUpdate
    - PhysicsUpdate
    - Physics2DUpdate
    - CheckTexFieldInput
    - IMGUISendQueuedEvents
    - NewInputUpdate
    - SendMouseEvents
    - AIUpdate
    - WindUpdate
    - UpdateVideo
    #Update
    - ScriptRunBehaviourUpdate
    - ScriptRunDelayedDynamicFrameRate
    - DirectorUpdate
    #PreLateUpdate
    - AIUpdatePostScript
    - DirectorUpdateAnimationBegin
    - LegacyAnimationUpdate
    - DirectorUpdateAnimationEnd
    - DirectorDeferredEvaluate
    - UpdateNetworkManager
    - UpdateMasterServerInterface
    - UNetUpdate
    - EndGraphicsJobsLate
    - ParticleSystemBeginUpdateAll
    - ScriptRunBehaviourLateUpdate
    - ConstraintManagerUpdate
    #PostLateUpdate
    - PlayerSendFrameStarted
    - DirectorLateUpdate
    - ScriptRunDelayedDynamicFrameRate
    - PhysicsSkinnedClothBeginUpdate
    - UpdateCanvasRectTransform
    - PlayerUpdateCanvases
    - UpdateAudio
    - ParticlesLegacyUpdateAllParticleSystems
    - ParticleSystemEndUpdateAll
    - UpdateCustomRenderTextures
    - UpdateAllRenderers
    - EnlightenRuntimeUpdate
    - UpdateAllSkinnedMeshes
    - ProcessWebSendMessages
    - SortingGroupsUpdate
    - UpdateVideoTextures
    - UpdateVideo
    - DirectorRenderImage
    - PlayerEmitCanvasGeometry
    - PhysicsSkinnedClothFinishUpdate
    - FinishFrameRendering
    - BatchModeUpdate
    - PlayerSendFrameComplete
    - UpdateCaptureScreenshot
    - PresentAfterDraw
    - ClearImmediateRenderers
    - PlayerSendFramePostPresent
    - UpdateResolution
    - InputEndFrame
    - TriggerEndOfFrameCallbacks
    - GUIClearEvents
    - ShaderHandleErrors
    - ResetInputAxis
    - ThreadedLoadingDebug
    - ProfilerSynchronizeStats
    - MemoryFrameMaintenance
    - ExecuteGameCenterCallbacks
    - ProfilerEndFrame
