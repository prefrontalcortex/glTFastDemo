﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GLTFast;
using UnityEngine;

public class CustomLoadDemo : MonoBehaviour {
    
    public string[] manyUrls;
    
    // Start is called before the first frame update
    async void Start() {
        var gltf = new GLTFast.GLTFast();
        var success = await gltf.Load("file:///path/to/file.gltf");

        if (success) {
            // Get the first material
            var material = gltf.GetMaterial();
            Debug.LogFormat("The first material is called {0}", material.name);

            // Instantiate the scene multiple times
            gltf.InstantiateGltf( new GameObject("Instance 1").transform );
            gltf.InstantiateGltf( new GameObject("Instance 2").transform );
            gltf.InstantiateGltf( new GameObject("Instance 3").transform );
        } else {
            Debug.LogError("Loading glTF failed!");
        }
    }
    
    async void CustomDeferAgent() {
        // Recommended: Use a common defer agent across multiple GLTFast instances!
        
        // For a stable frame rate:
        IDeferAgent deferAgent = gameObject.AddComponent<TimeBudgetPerFrameDeferAgent>();
        // Or for faster loading:
        deferAgent = new UninterruptedDeferAgent();

        var tasks = new List<Task>();
        
        foreach( var url in manyUrls) {
            var gltf = new GLTFast.GLTFast(null,deferAgent);
            var task = gltf.Load(url).ContinueWith(t => {
                if (t.Result) {
                    gltf.InstantiateGltf(transform);
                }
            });
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}
