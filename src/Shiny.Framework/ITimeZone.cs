namespace Shiny;

public class ITimeZone
{
    
}
/*
 https://stackoverflow.com/questions/27053135/how-to-get-a-users-time-zone
 
Objective-C
   NSTimeZone *timeZone = [NSTimeZone localTimeZone];
   NSString *tzName = [timeZone name];
   Swift
   let tzName = TimeZone.current.identifier
   
   var gmtHoursOffset : String = String()
       
   override func viewDidLoad() {
       super.viewDidLoad()
       gmtHoursOffset = getHoursFromGmt()
       print(gmtHoursOffset)
   }
   
   func getHoursFromGmt() -> String {
       let secondsFromGmt: Int = TimeZone.current.secondsFromGMT()
       let hoursFromGmt = (secondsFromGmt / 3600)
       return "\(hoursFromGmt)"
   }
 */