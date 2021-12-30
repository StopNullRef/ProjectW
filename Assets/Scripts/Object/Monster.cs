using ProjectW.DB;
using ProjectW.Object;
using ProjectW.UI;
using ProjectW.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectW.Object
{
    /// <summary>
    /// ���ʹ� �ΰ��� ���߿� ����� �����ǰ�, ���ŵǹǷ� ������Ʈ Ǯ�� �̿��Ѵ�.
    /// -> ������Ʈ�� Ǯ�� �̿��� ��ü���Դ� ������Ʈ Ǯ ����� ������ �� �ְ� IPoolableObject �������̽��� ��ӽ�Ų��.
    /// </summary>
    public class Monster : Actor, IPoolableObject
    {
        public BoMonster boMonster;

        private NavMeshAgent agent;
        private NavMeshPath path;

        // ���� ���� ���θ� ��Ÿ���� ������Ƽ, ó�� ���� �� ���� ������ ���·� ����
        public bool CanRecycle { get; set; } = true;

        public override void Initialize(BoActor boActor)
        {
            base.Initialize(boActor);
            // ������ ���Ǹ� ���� BoMonster ���·� ĳ�����Ͽ� ��Ƶ�
            boMonster = boActor as BoMonster;

            // �ʱ� ���� ����
            SetStats();
            // ���� ��� �ð� �ʱ�ȭ
            InitPatrolWaitTime();

            // ó�� �������� ���� �ڱ� �ڽ��� ��ġ�� �����Ѵ�.
            // ����? ó���� ������ �����Ͱ� ������� (�⺻ ���� 0,0,0�� �������� ��)
            // �������� ������ ���� ��ġ�� ���������ν�, ������ ���� üũ ������ ����
            // �ٷ� ���� ���·� �ν��ϰ� �Ͽ�, ������ ���ο� �������� ������ �� �ְ� �ϱ� ���ؼ�
            boMonster.desPos = transform.position;
        }

        protected override void Start()
        {
            base.Start();

            agent = GetComponent<NavMeshAgent>();
            path = new NavMeshPath();
        }

        public override void SetStats()
        {
            if (boMonster == null)
                return;

            boMonster.level = 1f;
            boMonster.actorType = Define.Actor.Type.Monster;
            boMonster.moveSpeed = boMonster.sdMonster.moveSpeed;
            boMonster.currentHp = boMonster.maxHp = boMonster.sdMonster.maxHp;
            boMonster.currentMp = boMonster.maxMp = boMonster.sdMonster.maxMp;
            boMonster.atkRange = boMonster.sdMonster.atkRange;
            boMonster.atkInterval = boMonster.sdMonster.atkInterval;
            boMonster.atk = boMonster.sdMonster.atk;
            boMonster.def = boMonster.sdMonster.def;
        }

        public override void ActorUpdate()
        {
            CheckDetection();

            base.ActorUpdate();
        }

        public override void MoveUpdate()
        {
            // ���Ͱ� ������ �� �ִ� �������� Ȯ��
            var isMove = GetMovement();

            // ������ �� �ִٸ�
            if (isMove)
            {
                // ���� ���̰ų� Ÿ���� �������̹Ƿ� �̵����·� ����
                SetState(Define.Actor.State.Walk);

                // �ӷ� ����
                agent.speed = boMonster.moveSpeed;
                // ������ ����
                agent.SetDestination(boMonster.desPos);
            }
            // ������ �� ���ٸ�
            else
            {
                // ���� �������̹Ƿ� ��� ���·� ����
                SetState(Define.Actor.State.Idle);
            }
        }

        /// <summary>
        /// ��Ȳ�� ���� �������� �����ϰ� ������ ���θ� ��ȯ
        /// </summary>
        /// <returns></returns>
        private bool GetMovement()
        {
            // Ÿ�ٿ� ���� ������ ������ �ִ���?
            if (attackController.hasTarget)
            {
                // ���� ���� ���� ĳ���Ͱ� �ִٸ� true, �ƴ϶�� false
                // !canAtk�� ������ �����ϸ� ������ ���θ� false, ���� �Ұ����� ���¶�� true
                return !attackController.canAtk;
            }

            // ���°� �����¶��
            if (State == Define.Actor.State.Idle)
            {
                // ���� �� ���ʹ� �ش� ��ġ���� �����
                // �� �� ���ð��� �����ٸ�
                if (Time.time - boMonster.startPatrolWaitTime > boMonster.patrolWaitTime)
                {
                    // ��Ⱑ �������Ƿ� �̵��� �� �ְ� true ��ȯ
                    return true;
                }

                // ���ð��� ������ �ʾ����Ƿ� �̵��� �� ���� false ��ȯ
                return false;
            }

            // ���� ��ġ�� �����ߴٸ� ���� ��ġ�� ����
            // false�� ��ȯ���Ѽ� �ٷ� �̵��ϴ� ���� �ƴ� ������ ���ð� ���� ��� �� �̵��ϵ���

            var distance = (boMonster.desPos - transform.position).magnitude;

            if (distance < agent.stoppingDistance)
            {
                // �������� ���������Ƿ� ���� ���ð� �ʱ�ȭ
                InitPatrolWaitTime();

                // ���� ��ġ ����
                ChangeDestPos();
                // ���� ��ġ�� ���������Ƿ�, ������ �� ������ false ��ȯ
                return false;
            }

            // �� �ڵ尡 ����Ǿ��ٴ� ���� ���� ���� ���̶�� ��
            // -> ���� �� �̵��� �ؾ��ϹǷ� true ��ȯ
            return true;

        }

        /// <summary>
        /// ���� ���ð� �ʱ�ȭ
        /// </summary>
        private void InitPatrolWaitTime()
        {
            boMonster.startPatrolWaitTime = Time.time;
            // ���� ���ð��� �� �� �����ϰ� ����
            boMonster.patrolWaitTime = Random.Range(Define.Monster.MinPatrolWaitTime, Define.Monster.MaxPatrolwaitTime);
        }

        /// <summary>
        /// ���� ��ġ�� �����ϴ� ���
        /// </summary>
        private void ChangeDestPos()
        {
            // ���͸��� ���� ������ �ٸ�����, ������ �ε��� ���� �Ѱ� �ش� ������
            // ���� ���� ������ ������ ��ġ�� �޾ƿ´�.
            boMonster.desPos = InGameManager.Instance.GetRandPosInArea(boMonster.sdMonster.index);

            // ���� ������ ���� ���� ���� ������ ������ ��ġ�� �̾���
            // ����ó��
            // 1. ���� ��ġ�� �׺�޽� ��� �� �����ϴ��� Ȯ��
            // 2. ���� ��ġ�� �׺�޽� ��λ� ����������, �������� ������ �� ���� ��������� Ȯ��

            var isExist = agent.CalculatePath(boMonster.desPos, path);

            // �ش� ��ġ�� �׺�޽��� �������� �ʴ� ���
            if (!isExist)
            {
                // ��� ȣ���� ���� ������ġ�� ���� �̰� ��θ� �ٽ� �˻�
                ChangeDestPos();
            }
            // �ش� ��ġ�� �׺�޽��� ����������, �������� ������  �� ���� ���
            else if (path.status == NavMeshPathStatus.PathPartial)
            {
                ChangeDestPos();
            }
        }

        /// <summary>
        /// �� ���� ���� ���� ���� �����ϴ��� üũ
        /// </summary>
        private void CheckDetection()
        {
            // ��ȹ�����Ϳ� ������ �������� ������ x,y,z������ ������ ���� �ڽ������� ����
            var value = boMonster.sdMonster.detectionRange;
            // �� �� �ڽ� ������ detectionRange�� ���� 2�谡 �ȴ�.
            var halfExtents = new Vector3(value, value, value);

            // ������ �ڽ� ���� �ȿ� �÷��̾� ���̾ ���� ��ü�� ��ħ�� �߻��ߴ��� Ȯ��
            // -> ��ħ�� �߻��ߴٸ� ��ħ�� �߻��� �ݶ��̴��� ������ ��ȯ�ȴ�.
            var colls = Physics.OverlapBox(transform.position, halfExtents, transform.rotation, 1 << LayerMask.NameToLayer("Player"));

            if (colls.Length == 0)
            {
                attackController.hasTarget = false;
                return;
            }

            // �� ���� ���Դٴ� ���� �÷��̾ �����ߴٴ� ��
            attackController.hasTarget = true;
            // �÷���� ������ �� �ֵ��� �������� �÷��̾�� ����
            boMonster.desPos = colls[0].transform.position;

            // �÷��̾���� �Ÿ���  ���� ���� ���̶�� ���ݰ��� ���·� ����
            var distance = boMonster.desPos - transform.position;
            // �÷��̾���� �Ÿ��� ���ݹ��� ���̶��
            if (distance.magnitude <= boMonster.atkRange)
            {
                // ���� ���� ���·� ����
                attackController.canAtk = true;
                // ���� ���� ���°� �Ǹ� �̵����� ������ �켱�� �Ǿ� �̵��̳� ȸ���� ���� �ʴ´�.
                // �� ��, ĳ���Ͱ� �̵� �ÿ��� ���Ͱ� ĳ���͸� �ٶ� �� �ֵ��� ȸ������ ����
                transform.rotation = Quaternion.LookRotation(distance.normalized);
            }
            else
            {
                // ���� �Ұ��� ���·� ����
                attackController.canAtk = false;
            }
        }

        /// <summary>
        /// ������ ��� ����� ������� ��, �߻���ų �̺�Ʈ
        /// </summary>
        public override void OnDeadEnd()
        {
            // ���� Ǯ�� ���͸� �ٽ� ����
            ObjectPoolManager.Instance.GetPool<Monster>().Return(this);

            // ���⼭ ������ ������ֱ�
            ItemDrop();
        }

        public void ItemDrop()
        {
           var uiIngame = UIWindowManager.Instance.GetWindow<UIIngame>();

            var sdMonster = boMonster.sdMonster;
            //������ ��� ���� ����

            for (int i=0; i<boMonster.sdMonster.dropItemRef.Length;i++)
            {
                if(Random.Range(1,100) <= (sdMonster.dropItemPer[i]*100))
                {
                    uiIngame.AddItem(this, new BoItem(GameManager.SD.sdItems.Where(_ => _.index == sdMonster.dropItemRef[i]).SingleOrDefault()));
                }
            }

            //uiIngame.AddItem(this,);

        }

    }
}
