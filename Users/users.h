#ifndef Users_h
#define Users_h
#include <string>

class Users{
	public:
	std::string name;
	
	Users();
	set_pass(std::string ps);
	
	private:
	std::string pass;
}; 

#endif