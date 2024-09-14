using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomralBullet : BaseBullet
{
    private float bulletSpeed;
    private float maxBulletSpeed;
    private float traMaxRelativeHeight;
    private Transform target;
    private AnimationCurve traAniCurve;
    private AnimationCurve axisCorrectionAniCurve;
    private AnimationCurve speedAniCurve;

    private Vector3 traStartPoint;
    private Vector3 bulletMoveDir;

    private void Start()
    {
        traStartPoint = transform.position;
    }

    void Update()
    {
        UpdateBulletPosition();
        /*Vector3 moveDirNormalized = (target.position - transform.position).normalized;
        transform.position += moveDirNormalized * bulletSpeed * Time.deltaTime;*/

        if (transform.position.x < -100)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateBulletPosition()
    {
        Vector3 traRange = target.position - traStartPoint;

        if(traRange.x < 0)
        {
            bulletSpeed = -bulletSpeed;
        }

        float nextPositionX = transform.position.x + bulletSpeed * Time.deltaTime; // 下一个x坐标点在世界中的位置
        float nextPositionXNormalized = (nextPositionX - traStartPoint.x) / traRange.x; //下一个x坐标点在曲线中的绝对位置

        float nextPositionYNormalized = traAniCurve.Evaluate(nextPositionXNormalized); //通过x坐标点的位置，倒推出y坐标点在曲线中的位置
        float nextPositionYCorrectionNormalized = axisCorrectionAniCurve.Evaluate(nextPositionXNormalized);

        float nextPositionYCorrection = nextPositionYCorrectionNormalized * traRange.y;

        float nextPositionY = traStartPoint.y + nextPositionYNormalized * traMaxRelativeHeight + nextPositionYCorrection; // 下一个y坐标点在世界中的位置

        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);

        UpdateBulletSpeed(nextPositionXNormalized);
        bulletMoveDir = newPosition - transform.position;

        transform.position = newPosition;
    }


    private void UpdateBulletSpeed(float nextPositionXNormalized)
    {
        float nextMoveSpeedNomralized = speedAniCurve.Evaluate(nextPositionXNormalized);

        bulletSpeed = nextMoveSpeedNomralized * maxBulletSpeed; 
    }


    public override void InitializeBullet(Transform target, float maxBulletSpeed, float traMaxHeight)
    {
        this.target = target;
        this.maxBulletSpeed = maxBulletSpeed;

        float xDistanceToTarget = target.position.x - transform.position.x;
        this.traMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * traMaxHeight;
    }

    public override void InitializeAniCurve(AnimationCurve traAniCurve, AnimationCurve axisCorrectionAniCurve, AnimationCurve speedAniCurve)
    {
        this.traAniCurve = traAniCurve;
        this.axisCorrectionAniCurve = axisCorrectionAniCurve;
        this.speedAniCurve = speedAniCurve;
    }

    public override Vector3 GetBulletMoveDir()
    { 
    return bulletMoveDir;
    }

}
