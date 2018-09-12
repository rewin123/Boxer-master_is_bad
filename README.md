# Boxer

Welcome to the Boxer repository!

At work, they were given the task of improving the recognition of blood agglutination by image scans using C #. Thus, neural networks for C# were written. They gave a result in 98 percent of the correct answers!

Well, in this repository you will find a solution in the Boxer folder, training and validation data in the other two folders. All the core of neural networks is in the NeuralNetwork folder. And visual interaction you can run by running NetworkLab (but it's in Russian).

The basis of this lib are several basic classes and interfaces:  
Network - the feedforward neural network itself  
Layer - one layer of the network  
Activation - network activation function   
IDataEnumerator - input for network training and network validation  
Optimizer - gradient descent method

In this lib doesn't solve "vanish" gradient:( But pretrain give good result. I got 95% with this code:

            BitmapCatEnumerator enums = new BitmapCatEnumerator("Sorted", new System.Drawing.Size(200, 130));
            BitmapCatEnumerator val = new BitmapCatEnumerator("Val", new System.Drawing.Size(200, 130));
            Network network = new Network();
            network.AddLayer(new Conv2D(new Relu(), 5, 5, 8));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 5, 5, 16));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 32));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 54));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new Conv2D(new Relu(), 3, 3, 80));
            network.AddLayer(new MaxPool2D(new Relu(), 2, 2));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 100)));
            network.AddLayer(new FullyConnLayar(new Sigmoid(), new NeuralNetwork.Size(1, 1, 2)));

            network.Compile(new Size(3, 130, 200), true);

            network.Normalization();
            network.Normalization();

            MomentumParallel sgd = new MomentumParallel(network, 0.9, 1e-2);
            sgd.need_max = false;

            var pair = FullyConPretrain.Action(network, sgd, enums, val, 1000, 64);

