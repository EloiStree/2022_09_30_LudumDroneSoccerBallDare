using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TNet;


[RequireComponent(typeof(TNObject))]
public class TN_RCRemotePlayer : TNBehaviour
{
	public RCControllerValueMono m_inputValue;

    public void Update()
	{
		tno.SendQuickly(0, Target.OthersSaved, m_inputValue.GetTiltPercent() );
		tno.SendQuickly(1, Target.OthersSaved, m_inputValue.GetRollPercent() );
		tno.SendQuickly(2, Target.OthersSaved, m_inputValue.GetThrottlePercent() );
		tno.SendQuickly(3, Target.OthersSaved, m_inputValue.GetYawPercent() );
	}
		
	[RFC(0)] void SetTiltPercent(float percent) { m_inputValue.SetTiltPercent(percent); }
	[RFC(1)] void SetRollPercent(float percent) { m_inputValue.SetRollPercent(percent); }
	[RFC(2)] void SetThrottlePercent(float percent) { m_inputValue.SetThrottlePercent(percent); }
	[RFC(3)] void SetYawPercent(float percent) { m_inputValue.SetYawPercent(percent); }
	
}
