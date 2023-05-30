using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Stove.PCSDK.NET;
using UnityEngine;
using System.Text;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class StovePCSDKManager : MonoBehaviour
{
    private static StovePCSDKManager instance;
    public static StovePCSDKManager Instance => instance;

    public bool useStoveSDK = false;
    
    private StovePCCallback callback;
    private Coroutine runCallbackCoroutine;

    [HideInInspector] public ulong LOGIN_USER_MEMBER_NO;
    // LoadConfig를 통해 채워지는 설정값
    private string Env = "live";
    private string AppKey = "c31a814c6f01baaaa01765cde53df5cc623b6e7ac42794842fa87ca5a1d33b23";
    private string AppSecret = "17e499f02999cc270682b1a94a503c777aede8a8b315c80e737678fc78515bf9b3ea64bfcb9e3f6051b360694efa76d5";
    private string GameId = "NTIMES_IND_DEMO_01_IND";
    private StovePCLogLevel LogLevel = StovePCLogLevel.Debug;
    private string LogPath = "";

    [SerializeField] private CanvasGroup logCG;
    [SerializeField] private TMP_Text log_text;
    private Sequence logSequence;

    private void Awake()
    {
        logSequence = DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() => { logCG.alpha = 0; })
            .Append(logCG.DOFade(1, 0.25f).From(0))
            .AppendInterval(3f)
            .Append(logCG.DOFade(0, 0.25f))
            .OnComplete(() => { logCG.alpha = 0; });
        logSequence.Complete();
        
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        if (!useStoveSDK)
        {
            WriteLog("StoveSDK를 사용하지 않습니다.");
            InitLoading.GoToMainTitle();
            return;
        }
        Initialize();
        
    }

    private void OnDestroy()
    {
        UnInitialize();
    }

    // private void ButtonLoadConfig_Click() // 게임 정보 불러오기
    // {
    //     string configFilePath = Application.streamingAssetsPath + "/Text/StovePCConfig.Unity.txt";
    //
    //     if (File.Exists(configFilePath))
    //     {
    //         string configText = File.ReadAllText(configFilePath);
    //         StovePCConfig config = JsonUtility.FromJson<StovePCConfig>(configText);
    //
    //         this.Env = config.Env;
    //         this.AppKey = config.AppKey;
    //         this.AppSecret = config.AppSecret;
    //         this.GameId = config.GameId;
    //         this.LogLevel = config.LogLevel;
    //         this.LogPath = config.LogPath;
    //
    //         WriteLog(configText);
    //     }
    //     else
    //     {
    //         string msg = String.Format("File not found : {0}", configFilePath);
    //         WriteLog(msg);
    //     }
    // }

    public void Initialize() // 초기화
    {
        WriteLog("Stove 설정 초기화중...");
        StovePCResult sdkResult = StovePCResult.NoError;

        StovePCConfig config = new StovePCConfig
        {
            Env = this.Env,
            AppKey = this.AppKey,
            AppSecret = this.AppSecret,
            GameId = this.GameId,
            LogLevel = this.LogLevel,
            LogPath = this.LogPath
        };

        this.callback = new StovePCCallback
        {
            OnError = new Stove.PCSDK.NET.StovePCErrorDelegate(this.OnError),
            OnInitializationComplete = new StovePCInitializationCompleteDelegate(this.OnInitializationComplete),
            // OnOwnership = new StovePCOwnershipDelegate(this.OnOwnership),
            OnToken = new StovePCTokenDelegate(this.OnToken),
            OnUser = new StovePCUserDelegate(this.OnUser),
            
            // 게임지원서비스
            OnStat = new StovePCStatDelegate(this.OnStat),
            OnSetStat = new StovePCSetStatDelegate(this.OnSetStat),
            // OnAchievement = new StovePCAchievementDelegate(this.OnAchievement),
            // OnAllAchievement = new StovePCAllAchievementDelegate(this.OnAllAchievement),
            OnRank = new StovePCRankDelegate(this.OnRank)
        };
        
        sdkResult = StovePC.Initialize(config, callback);
        
        if (StovePCResult.NoError == sdkResult)
        {
            this.runCallbackCoroutine = StartCoroutine(RunCallback(0.5f));
            // 초기화 오류가 없어 RunCallback 주기적 호출 -> 다른 CallBack들을 호출할 수 있게 함
        }
        else
        {
            // 초기화 실패로 게임 종료
            UnityEngine.Debug.Log(sdkResult.ToString());
        }
    }

    public void UnInitialize() // 초기화 종료
    {
        if(runCallbackCoroutine != null)
            StopCoroutine(runCallbackCoroutine);
        StovePCResult result = StovePC.Uninitialize();
        if (result == StovePCResult.NoError)
        {
            // 성공 처리
            UnityEngine.Debug.Log("UnInitialized");
        }
    }

    #region USER
    [HideInInspector] public UnityEvent<StovePCUser> OnUserEvent;
    public void GetUserMethod()
    {
        StovePCResult result = StovePC.GetUser();
        if (result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    void OnUser(StovePCUser user)
    {
        OnUserEvent.Invoke(user);
        LOGIN_USER_MEMBER_NO = user.MemberNo;
        // 사용자 정보 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnUser");    
        sb.AppendFormat(" - user.MemberNo : {0}" + Environment.NewLine, user.MemberNo.ToString());
        sb.AppendFormat(" - user.Nickname : {0}" + Environment.NewLine, user.Nickname);
        sb.AppendFormat(" - user.GameUserId : {0}", user.GameUserId);
    
        Debug.Log(sb.ToString());
    }
    #endregion

    #region TOKEN
    [HideInInspector] public UnityEvent<StovePCToken> OnTokenEvent;
    public void GetTokenMethod()
    {
        StovePCResult result = StovePC.GetToken();
        if (result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    
    void OnToken(StovePCToken token)
    {
        OnTokenEvent.Invoke(token);
        // 토큰 정보 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnToken");
        sb.AppendFormat(" - token.AccessToken : {0}", token.AccessToken);

        Debug.Log(sb.ToString());
    }
    #endregion
    
    // #region OWNERSHIP
    // [Button]
    // public void GetOwnershipMethod()
    // {
    //     StovePCResult result = StovePC.GetOwnership();
    //     if (result == StovePCResult.NoError)
    //     {
    //         // 성공 처리
    //     }
    // }
    // void OnOwnership(StovePCOwnership[] ownerships)
    // {
    //     bool owned = false;
    //
    //     foreach(var ownership in ownerships)
    //     {
    //         // [LOGIN_USER_MEMBER_NO] StovePCUser 구조체의 MemberNo
    //         // [OwnershipCode] 1: 소유권 획득, 2: 소유권 해제(구매 취소한 경우)
    //         if (ownership.MemberNo != LOGIN_USER_MEMBER_NO ||
    //             ownership.OwnershipCode != 1)
    //         {
    //             continue;
    //         }
    //
    //         // [GameCode] 3: BASIC 게임, 5: DLC
    //         if (ownership.GameId == "YOUR_GAME_ID" &&
    //             ownership.GameCode == 3)
    //         {
    //             owned = true; // 소유권 확인 변수 true로 설정
    //         }
    //
    //         // DLC를 판매하는 게임일 때만 필요
    //         if (ownership.GameId == "YOUR_DLC_ID" &&
    //             ownership.GameCode == 5)
    //         {
    //             // YOUR_DLC_ID(DLC) 소유권이 있기에 DLC 플레이 허용
    //         }
    //     }
    //  
    //     if(owned)
    //     {
    //         // 소유권 검증이 정상적으로 완료 된 이후 게임진입 로직 작성
    //         UnityEngine.Debug.Log("owned");
    //     }
    //     else
    //     {
    //         // 소유권 검증실패 후 게임을 종료하고 에러메세지 표출 로직 작성
    //         UnityEngine.Debug.Log("not owned");
    //     }
    // }
    // #endregion

    #region ERROR
    private void OnError(StovePCError error)
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnError");
        sb.AppendFormat(" - error.FunctionType : {0}" + Environment.NewLine, error.FunctionType.ToString());
        sb.AppendFormat(" - error.Result : {0}" + Environment.NewLine, (int)error.Result);
        sb.AppendFormat(" - error.Message : {0}" + Environment.NewLine, error.Message);
        sb.AppendFormat(" - error.ExternalError : {0}", error.ExternalError.ToString());
        UnityEngine.Debug.Log(sb.ToString());
        #endregion

        switch (error.FunctionType)
        {
            case StovePCFunctionType.Initialize:
            case StovePCFunctionType.GetUser:
            case StovePCFunctionType.GetOwnership:
                BeginQuitAppDueToError();
                break;
        }
    }
    #endregion

    #region STAT
    public void GetStatMethod(string STAT_ID)
    {
        // 입력 파라미터
        // string statId : 스튜디오에서 등록한 스탯 식별자
        StovePCResult result = StovePC.GetStat(STAT_ID);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }

    public void SetRecordLevel(int STAT_VALUE)
    {
        // 입력 파라미터
        // string statId : 스튜디오에서 등록한 스탯 식별자
        // int statValue : 업데이트할 스탯값
        StovePCResult result = StovePC.SetStat("RECORD_LEVEL", STAT_VALUE);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    public void SetGrowthLevel(int STAT_VALUE)
    {
        // 입력 파라미터
        // string statId : 스튜디오에서 등록한 스탯 식별자
        // int statValue : 업데이트할 스탯값
        StovePCResult result = StovePC.SetStat("GROWTH_LEVEL", STAT_VALUE);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }

    public void SetStatMethod(string STAT_ID, int STAT_VALUE)
    {
        // 입력 파라미터
        // string statId : 스튜디오에서 등록한 스탯 식별자
        // int statValue : 업데이트할 스탯값
        StovePCResult result = StovePC.SetStat(STAT_ID, STAT_VALUE);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    
    private void OnStat(StovePCStat stat)
    {
        // 스탯 정보 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnStat");
        sb.AppendFormat(" - stat.StatFullId.GameId : {0}" + Environment.NewLine, stat.StatFullId.GameId);
        sb.AppendFormat(" - stat.StatFullId.StatId : {0}" + Environment.NewLine, stat.StatFullId.StatId);
        sb.AppendFormat(" - stat.MemberNo : {0}" + Environment.NewLine, stat.MemberNo.ToString());
        sb.AppendFormat(" - stat.CurrentValue : {0}" + Environment.NewLine, stat.CurrentValue.ToString());
        sb.AppendFormat(" - stat.UpdatedAt : {0}", stat.UpdatedAt.ToString());
 
        Debug.Log(sb.ToString());
    }
    private void OnSetStat(StovePCStatValue statValue)
    {
        // 스탯 업데이트 결과 정보 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnSetStat");
        sb.AppendFormat(" - statValue.CurrentValue : {0}" + Environment.NewLine, statValue.CurrentValue.ToString());
        sb.AppendFormat(" - statValue.Updated : {0}" + Environment.NewLine, statValue.Updated.ToString());
        sb.AppendFormat(" - statValue.ErrorMessage : {0}", statValue.ErrorMessage);
   
        Debug.Log(sb.ToString());
    }
    #endregion

    // #region ACHIEVEMENT
    // private void OnAchievement(StovePCAchievement achievement)
    // {
    //     // 단일 업적 정보 출력
    //     StringBuilder sb = new StringBuilder();
    //     sb.AppendLine("OnAchievement");
    //     sb.AppendFormat(" - achievement.AchievementId : {0}" + Environment.NewLine, achievement.AchievementId);
    //     sb.AppendFormat(" - achievement.Name : {0}" + Environment.NewLine, achievement.Name);
    //     sb.AppendFormat(" - achievement.Description : {0}" + Environment.NewLine, achievement.Description);
    //     sb.AppendFormat(" - achievement.DefaultImage : {0}" + Environment.NewLine, achievement.DefaultImage);
    //     sb.AppendFormat(" - achievement.AchievedImage : {0}" + Environment.NewLine, achievement.AchievedImage);
    //     sb.AppendFormat(" - achievement.Condition.GoalValue : {0}" + Environment.NewLine, achievement.Condition.GoalValue.ToString());
    //     sb.AppendFormat(" - achievement.Condition.ValueOperation : {0}" + Environment.NewLine, achievement.Condition.ValueOperation);
    //     sb.AppendFormat(" - achievement.Condition.Type : {0}" + Environment.NewLine, achievement.Condition.Type);
    //     sb.AppendFormat(" - achievement.Value : {0}" + Environment.NewLine, achievement.Value.ToString());
    //     sb.AppendFormat(" - achievement.Status : {0}", achievement.Status);
    //
    //     Debug.Log(sb.ToString());
    // }
    //
    // private void OnAllAchievement(StovePCAchievement[] achievements)
    // {
    //     // 모든 업적 정보 출력
    //     StringBuilder sb = new StringBuilder();
    //     sb.AppendLine("OnAllAchievement");
    //     sb.AppendFormat(" - achievements.Length : {0}" + Environment.NewLine, achievements.Length);
    //
    //     for (int i = 0; i < achievements.Length; i++)
    //     {
    //         sb.AppendFormat(" - achievements[{0}].AchievementId : {1}" + Environment.NewLine, i, achievements[i].AchievementId);
    //         sb.AppendFormat(" - achievements[{0}].Name : {1}" + Environment.NewLine, i, achievements[i].Name);
    //         sb.AppendFormat(" - achievements[{0}].Description : {1}" + Environment.NewLine, i, achievements[i].Description);
    //         sb.AppendFormat(" - achievements[{0}].DefaultImage : {1}" + Environment.NewLine, i, achievements[i].DefaultImage);
    //         sb.AppendFormat(" - achievements[{0}].AchievedImage : {1}" + Environment.NewLine, i, achievements[i].AchievedImage);
    //         sb.AppendFormat(" - achievements[{0}].Condition.GoalValue : {1}" + Environment.NewLine, i, achievements[i].Condition.GoalValue.ToString());
    //         sb.AppendFormat(" - achievements[{0}].Condition.ValueOperation : {1}" + Environment.NewLine, i, achievements[i].Condition.ValueOperation);
    //         sb.AppendFormat(" - achievements[{0}].Condition.Type : {1}" + Environment.NewLine, i, achievements[i].Condition.Type);
    //         sb.AppendFormat(" - achievements[{0}].Value : {1}" + Environment.NewLine, i, achievements[i].Value.ToString());
    //         sb.AppendFormat(" - achievements[{0}].Status : {1}", i, achievements[i].Status);
    //
    //         if (i < achievements.Length - 1)
    //             sb.AppendFormat(Environment.NewLine);
    //     }
    //
    //     Debug.Log(sb.ToString());
    // }
    // #endregion

    #region RANK
    [HideInInspector] public UnityEvent<StovePCRank[], uint> OnRankEvent;
    public void GetRankMethod(string LEADERBOARD_ID, uint PAGE_INDEX, uint PAGE_SIZE, bool INCLUDE_MY_RANK)
    {
        // 입력 파라미터
        // string leaderboardId : 스튜디오에서 생성한 리더보드 식별자
        // uint pageIndex : 조회할 페이지 번호 (1 <= pageIndex)
        // uint pageSize : 조회할 순위의 개수 (1 <= pageSize <= 50)
        // bool includeMyRank : 조회결과에 로그인한 사용자의 순위를 포함할지 여부
        StovePCResult result = StovePC.GetRank(LEADERBOARD_ID, PAGE_INDEX, PAGE_SIZE, INCLUDE_MY_RANK);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }

    public void GetGrowthRank()
    {
        // 입력 파라미터
        // string leaderboardId : 스튜디오에서 생성한 리더보드 식별자
        // uint pageIndex : 조회할 페이지 번호 (1 <= pageIndex)
        // uint pageSize : 조회할 순위의 개수 (1 <= pageSize <= 50)
        // bool includeMyRank : 조회결과에 로그인한 사용자의 순위를 포함할지 여부
        StovePCResult result = StovePC.GetRank("NTIMES_IND_DEMO_01_IND|GROWTH_LEVEL", 
            1, 10, true);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    public void GetRecordRank()
    {
        // 입력 파라미터
        // string leaderboardId : 스튜디오에서 생성한 리더보드 식별자
        // uint pageIndex : 조회할 페이지 번호 (1 <= pageIndex)
        // uint pageSize : 조회할 순위의 개수 (1 <= pageSize <= 50)
        // bool includeMyRank : 조회결과에 로그인한 사용자의 순위를 포함할지 여부
        StovePCResult result = StovePC.GetRank("NTIMES_IND_DEMO_01_IND|RECORD_LEVEL", 
            1, 10, true);
        if(result == StovePCResult.NoError)
        {
            // 성공 처리
        }
    }
    // 콜백 파라미터
    // uint rankTotalCount : 조회한 리더보드에 집계된 전체 순위 개수
    private void OnRank(StovePCRank[] ranks, uint rankTotalCount)
    {
        OnRankEvent.Invoke(ranks, rankTotalCount);
        // 순위 정보 출력
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("OnRank");
        sb.AppendFormat(" - ranks.Length : {0}" + Environment.NewLine, ranks.Length);
 
        for (int i = 0; i < ranks.Length; i++)
        {
            sb.AppendFormat(" - ranks[{0}].MemberNo : {1}" + Environment.NewLine, i, ranks[i].MemberNo.ToString());
            sb.AppendFormat(" - ranks[{0}].Score : {1}" + Environment.NewLine, i, ranks[i].Score.ToString());
            sb.AppendFormat(" - ranks[{0}].Rank : {1}" + Environment.NewLine, i, ranks[i].Rank.ToString());
            sb.AppendFormat(" - ranks[{0}].Nickname : {1}" + Environment.NewLine, i, ranks[i].Nickname);
            sb.AppendFormat(" - ranks[{0}].ProfileImage : {1}" + Environment.NewLine, i, ranks[i].ProfileImage);
        }
 
        sb.AppendFormat(" - rankTotalCount : {0}", rankTotalCount);

        Debug.Log(sb.ToString());
    }

    #endregion
    
    #region LOG
    
    private void WriteLog(string log)
    {
        // Debug.Log(log + Environment.NewLine);
        log_text.text = log;
        logSequence.Restart();
    }
    #endregion
    
    private void BeginQuitAppDueToError() // 에러로 인한 종료
    {
        #region Log
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("BeginQuitAppDueToError");
        sb.AppendFormat(" - nothing");
        UnityEngine.Debug.Log(sb.ToString());
        #endregion

        // 어쩌면 당신은 즉시 앱을 중단하기보다는 사용자에게 앱 중단에 대한 메시지를 보여준 후
        // 사용자 액션(e.g. 종료 버튼 클릭)에 따라 앱을 중단하고 싶어 할지도 모릅니다.
        // 그렇다면 여기에 QuitApplication을 지우고 당신만의 로직을 구현하십시오.
        // 권장하는 필수 사전 작업 오류에 대한 메시지는 아래와 같습니다.
        // 한국어 : 필수 사전 작업이 실패하여 게임을 종료합니다.
        // 그 외 언어 : The required pre-task fails and exits the game.
        // QuitApplication();
        // Application.Quit();
        WriteLog("필수 사전 작업이 실패하여 게임을 종료합니다.");
        Invoke("QuitAppication", 5f);
    }

    private void QuitAppication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }

    private void OnInitializationComplete() // 정상적으로 초기 실행이 완료되었을 시 실행될 콜백
    {
        Debug.Log("PC SDK initialization success");
        // WriteLog("환영합니다!");
        InitLoading.GoToMainTitle();
        OnUserEvent.AddListener(HelloUser);
        GetUserMethod();
    }

    private void HelloUser(StovePCUser user)
    {
        OnUserEvent.RemoveListener(HelloUser);

        StringBuilder sb = new StringBuilder();
        sb.Append("환영합니다, ");
        sb.Append(user.Nickname);
        sb.Append("님!\n");
        
        WriteLog(sb.ToString());
    }

    private IEnumerator RunCallback(float intervalSeconds) // 주기적으로 실행할 콜백
    {
        WaitForSeconds wfs = new WaitForSeconds(intervalSeconds);
        while (true)
        {
            StovePC.RunCallback();
            UnityEngine.Debug.Log("Stove Callback");
            yield return wfs;
        }
    }
}
