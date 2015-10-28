public class Server {
    //TODO define some class variables.

    public Server(String inputFileName, String outputFileName) {
        //TODO constructor of server class.
        throw new RuntimeException("Server constructor not implemented");
    }

    public void run(){
        initialize();
        process();
    }

    private void initialize() {
        //TODO parse the input file.
        throw new RuntimeException("initialize not implemented");
    }

    private void process() {
        //TODO process operations from the operation queue, one at a time.
        throw new RuntimeException("process not implemented");
    }

    public static void main(String[] args){
        if(args.length != 2){
            System.out.println("Usage: java Server [input.txt] [output.txt]");
            System.exit(0);
        }
        Server server = new Server(args[0], args[1]);
        server.run();
    }
}
