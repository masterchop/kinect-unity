﻿using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private GameMode gameMode = GameMode.LimitedTime;

	[SerializeField]
	private float time = 10f;
	
	[SerializeField]
	private float targetDamage = 2f;
	
	[SerializeField]
	private int targetScore = 4;

	[SerializeField]
	private float life = 20f;

	private int score = 0;

	[SerializeField]
	private float rateOfTargetSpawn = 0.2f;

	private double lastTargetSpawnTime;

	public static GameManager Instance {
		get;
		private set;
	}

	public static int TargetScore;

	public static float TargetDamage;

	private void Awake() {
		if (Instance != null) {
			Debug.LogError("There is multiple instance of singleton GameManager");
			return;
		}
		
		Instance = this;
		
		TargetScore = targetScore;
		TargetDamage = targetDamage;
	}


	void Start () {
	}
	
	void Update () {
		RandomSpawn ();

		if(gameMode == GameMode.LimitedTime) {
			time -= Time.deltaTime;

			if (time <= 0) {
				// TODO end game
			}
		}
	}


	private void RandomSpawn() {
		if (this.rateOfTargetSpawn <= 0f) {
			return;
		}
		
		float durationBetweenTwoTargetsSpawn = 1f / this.rateOfTargetSpawn;

		if (Time.time < this.lastTargetSpawnTime + durationBetweenTwoTargetsSpawn) {
			return;
		}

		// chose random target type
		System.Array values = TargetType.GetValues(typeof(TargetType));
		TargetType targetType = (TargetType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
		// spawn !
		TargetsFactory.GetTarget(targetType);

		this.lastTargetSpawnTime = Time.time;
	}


	void OnEnable() {
		Target.targetPlayerCollision += OnTargetPlayerCollision;
	}
	
	void OnDisable() {
		Target.targetPlayerCollision -= OnTargetPlayerCollision;
	}

	void OnTargetPlayerCollision(object sender, EventArgs e) {

		if (gameMode == GameMode.LimitedLife) {
			life -= 1f;
			if (life <= 0) {
				// TODO end game
			}
		}
	}
}